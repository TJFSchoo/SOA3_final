using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Person;

namespace AvansDevOps.Backlog
{
    public class Task
    {
        private string _description;
        private ETaskState _state;
        private PersonModel _assignedPerson;

        public Task(string description, PersonModel assignedPerson)
        {
            _description = description;
            _assignedPerson = assignedPerson;
            _state = ETaskState.Todo;
        }

        public void SetDescription(string newDescription)
        {
            if (_state == ETaskState.Done)
                throw new NotSupportedException("Can't change description of task when tasked is marked as done");
            _description = newDescription;
        }

        public string GetDescription()
        {
            return _description;
        }

        public PersonModel GetAssignedPerson()
        {
            return _assignedPerson;
        }

        public void AssignPerson(PersonModel newAssignee)
        {
            if (_state == ETaskState.Done)
                throw new NotSupportedException(
                    "Can't change the task assignee once the task has been marked as done.");
            _assignedPerson = newAssignee;
        }

        public ETaskState GetState()
        {
            return _state;
        }

        public void NextState()
        {
            switch (_state)
            {
                case ETaskState.Todo:
                    _state = ETaskState.Active;
                    break;

                case ETaskState.Active:
                    _state = ETaskState.Done;
                    break;

                case ETaskState.Done:
                    throw new NotSupportedException("Task is already done. Can't go to next state");

                default:
                    throw new NotSupportedException("Can't find state.");

            }
        }

        public void PreviousState()
        {
            switch (_state)
            {
                case ETaskState.Todo:
                    throw new NotSupportedException("Task is already on todo. Can't go to previous state.");

                case ETaskState.Active:
                    _state = ETaskState.Todo;
                    break;

                case ETaskState.Done:
                    _state = ETaskState.Active;
                    break;

                default:
                    throw new NotSupportedException("Can't find state.");

            }
        }
    }
}
