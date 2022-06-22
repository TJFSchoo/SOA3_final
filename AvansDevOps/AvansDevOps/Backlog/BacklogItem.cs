﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Backlog.BacklogItemStates;
using AvansDevOps.Person;
using AvansDevOps.Sprint;
using AvansDevOps.Notification;

namespace AvansDevOps.Backlog
{
    public class BacklogItem : NotificationSubject
    {
        private string _name;
        private string _description;
        private List<Task> _tasks;
        private PersonModel _assignedPerson;
        private int _effort;
        private IBacklogItemState _state;
        private readonly BacklogModel _backlogReference;
        private ISprint _sprintReference;

        public BacklogItem(string name, string description, PersonModel assignedPerson, int effort, BacklogModel backlog)
        {
            _name = name;
            _description = description;
            _assignedPerson = assignedPerson;
            _effort = effort;
            _state = new TodoState(this);
            _backlogReference = backlog;
        }

        public void SetSprint(ISprint sprint)
        {
            _sprintReference = sprint;
        }

        public ISprint GetSprint()
        {
            return _sprintReference;
        }

        public BacklogModel GetBacklog()
        {
            return _backlogReference;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetDescription(string description)
        {
            _description = description;
        }

        public string GetDescription()
        {
            return _description;
        }

        public void SetEffort(int effort)
        {
            _effort = effort;
        }

        public int GetEffort()
        {
            return _effort;
        }

        public void AssignPerson(PersonModel newAssignee)
        {
            _assignedPerson = newAssignee;
        }

        public PersonModel GetAssignedPerson()
        {
            return _assignedPerson;
        }

        public void ChangeState(IBacklogItemState state)
        {
            // BR: The state of the backlogItem can only be changed once it has a sprint reference
            if (_sprintReference == null)
                throw new NotSupportedException(
                    "Can't change the state of a backlogItem because it is not in a sprint");
            _state = state;
            NotifyObservers();
        }

        public IBacklogItemState GetState()
        {
            return _state;
        }

        public List<Task> GetTasks()
        {
            return _tasks;
        }

        public void AddTask(Task task)
        {
            // BR: If a backlogItem has tasks, remove the assignedPerson, and create a new task for the assignedPerson with the description

            if (_assignedPerson  != null && task != null)
            {
                var convertedBacklogItemToTask = new Task(_description, _assignedPerson);
                _assignedPerson = null;
                _description = null;

                _tasks = new List<Task>() { convertedBacklogItemToTask };
            }


            if (_tasks == null)
            {
                _tasks = new List<Task>() { task };
            }
            else
            {
                _tasks.Add(task);
            }
        }

        public bool RemoveTask(Task task)
        {
            if (_tasks == null)
            {
                throw new NotSupportedException("There are no tasks in this backlogItem");
            }

            return _tasks.Remove(task);
        }
    }
}
