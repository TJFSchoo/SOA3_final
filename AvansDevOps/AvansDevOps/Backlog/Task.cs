using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using AvansDevOps.Person;
using System.Threading.Tasks;

namespace AvansDevOps.Backlog
{
    public class Task
    {
        private string _description;
        private static string TAG = "Task";

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
                throw new NotSupportedException("[" + TAG + "] " + "Unable to change description of task when tasked is marked as done");
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
                    "[" + TAG + "] " + "Unable to change task assignee once the task has been marked as done.");
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
                    throw new NotSupportedException("[" + TAG + "] " + "Task is on status Done. Unable to assign new state");

                default:
                    throw new NotSupportedException("[" + TAG + "] " + "Unable to retrieve state.");

            }
        }

        public void PreviousState()
        {
            switch (_state)
            {
                case ETaskState.Todo:
                    throw new NotSupportedException("[" + TAG + "] " + "Task is on status ToDo. Unable to assign old state.");

                case ETaskState.Active:
                    _state = ETaskState.Todo;
                    break;

                case ETaskState.Done:
                    _state = ETaskState.Active;
                    break;

                default:
                    throw new NotSupportedException("[" + TAG + "] " + "Unable to retrieve state.");

            }
        }
    }
}
