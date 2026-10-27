using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamimgDistanceMin = 3f;
    [SerializeField] private float roamimgTimerMax = 2f;

    [SerializeField] private float chaseDistance = 5f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float deathTime = 4f;

    private NavMeshAgent _navMeshAgent;
    private EnemyVisual _enemyVisual;
    private PlayerHealth _playerHealth;

    private State _currentState;
    private float _roamingTimer;
    private float _attackTimer;

    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private enum State
    {
        Idle,
        Roaming,
        Chasing
    }

    private void Start()
    {
        _startingPosition = transform.position;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _enemyVisual = GetComponent<EnemyVisual>();

        _currentState = startingState;
    }

    private void Update()
    {
        if (Hero.Instance == null)
        {
            return;
        }

        _playerHealth =
            Hero.Instance.GetComponent<PlayerHealth>();

        if (_playerHealth != null &&
            _playerHealth.IsDying)
        {
            _navMeshAgent.ResetPath();
            return;
        }

        float distanceToHero = Vector3.Distance(
            transform.position,
            Hero.Instance.transform.position
        );

        switch (_currentState)
        {
            case State.Idle:

                _attackTimer = 0f;

                if (distanceToHero <= chaseDistance)
                {
                    _currentState = State.Chasing;
                }

                break;

            case State.Roaming:

                _attackTimer = 0f;

                if (distanceToHero <= chaseDistance)
                {
                    _currentState = State.Chasing;
                    break;
                }

                _roamingTimer -= Time.deltaTime;

                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = roamimgTimerMax;
                }

                break;

            case State.Chasing:

                if (distanceToHero > chaseDistance)
                {
                    _currentState = State.Roaming;

                    _navMeshAgent.ResetPath();
                    _roamingTimer = 0;
                    _attackTimer = 0;

                    break;
                }

                if (distanceToHero <= attackDistance)
                {
                    _navMeshAgent.ResetPath();

                    ChangeFacingDirection(
                        transform.position,
                        Hero.Instance.transform.position
                    );

                    _enemyVisual.Attack();

                    _attackTimer += Time.deltaTime;

                    if (_attackTimer >= deathTime)
                    {
                        _playerHealth.StartDeath();
                    }

                    break;
                }

                _attackTimer = 0f;

                _navMeshAgent.SetDestination(
                    Hero.Instance.transform.position
                );

                ChangeFacingDirection(
                    transform.position,
                    Hero.Instance.transform.position
                );

                break;
        }
    }

    private void Roaming()
    {
        _startingPosition = transform.position;

        _roamPosition = GetRoamingPosition();

        ChangeFacingDirection(
            _startingPosition,
            _roamPosition
        );

        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + GetRandomDir() *
            UnityEngine.Random.Range(
                roamimgDistanceMin,
                roamingDistanceMax
            );
    }

    private Vector3 GetRandomDir()
    {
        return new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            0f
        ).normalized;
    }

    private void ChangeFacingDirection(
        Vector3 sourcePosition,
        Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}