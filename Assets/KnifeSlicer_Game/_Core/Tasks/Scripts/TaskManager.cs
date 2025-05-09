using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{
    public class TaskManager : MonoBehaviour
    {
        public event Action<string> OnTaskError;
        public event Action OnTasksExecuted;

        [SerializeField] private UnityEvent OnTasksFailed;

        [SerializeField] private bool _executeOnAwake;
        [SerializeField] private Task[] _tasks;

        public Task[] Tasks => _tasks;

        private Coroutine _executionCoroutine;

        private void OnValidate()
        {
            _tasks = GetComponentsInChildren<Task>();
        }

        private void Awake()
        {
            if (_executeOnAwake)
                StartCoroutine(Execute());
        }

        public void SetExecuteOnAwake(bool executeOnAwake) => _executeOnAwake = executeOnAwake;

        public IEnumerator Execute()
        {
            if (_executionCoroutine != null)
            {
                Debug.LogWarning($"Execution of {gameObject.name} has already started.");
                yield break;
            }

            _executionCoroutine = StartCoroutine(ExecuteInternal());
            yield return _executionCoroutine;
            _executionCoroutine = null;
        }

        private IEnumerator ExecuteInternal()
        {
            yield return null;
            foreach (Task task in _tasks.OrderBy(t => t.transform.GetSiblingIndex()))
            {
                yield return task.Execute(this);
            }

            OnTasksExecuted?.Invoke();
            _executionCoroutine = null;
        }

        public void Interrupt(string reason)
        {
            if (_executionCoroutine == null)
                return;
            Debug.Log($"Aborting tasks from {gameObject.name} due to:\n{reason}");
            StopCoroutine(_executionCoroutine);
            _executionCoroutine = null;
            OnTaskError?.Invoke(reason);
            OnTasksFailed?.Invoke();
        }

        private void OnDestroy()
        {
            if (_executionCoroutine != null)
            {
                StopCoroutine(_executionCoroutine);
                _executionCoroutine = null;
            }
        }
    }
}