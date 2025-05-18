using KnifeSlicer.Core;
using System;
using System.Collections;
using UnityEngine;

namespace KnifeSlicer.Tasks
{
    [RequireComponent(typeof(Task))]
    public abstract class TaskBase : BaseBehaviour, ITask
    {
        public event Action<TaskBase> OnTaskStarted;

        [SerializeField] private bool _waitForCompletion = true;

        private bool _announceTask;

        private string _taskMessage;

        [SerializeField] private bool _willExecute;

        private Task _parent;
        public string TaskMessage => _taskMessage;

        public IEnumerator Execute()
        {
            Started(this);
            Coroutine executeInternal = StartCoroutine(ExecuteInternal());
            if (!_waitForCompletion)
                yield break;
            yield return executeInternal;
        }

        public abstract IEnumerator ExecuteInternal();
        public virtual bool IsFinished() => true;

        public virtual bool CanExecute()
        {
            return true;
        }

        public void Started(TaskBase task)
        {
            OnTaskStarted?.Invoke(task);
        }

        private void OnValidate()
        {
            _willExecute = CanExecute();
            _parent ??= GetComponent<Task>();
        }

        protected void InterruptWithError(string error)
        {
            if (_parent == null)
                _parent = GetComponent<Task>();
            _parent.InterruptWithError(error);
        }
    }
}