using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Events.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public sealed class Employee : Entity
    {
        public EmployeeFirstName FirstName { get; }
        public EmployeeMiddleName MiddleName { get; }
        public EmployeeLastName LastName { get; }
        public BirthDay BirthDay { get; }
        public HiringDate HiringDate { get; }
        public Email Email { get; }
        public bool IsDismissed { get; private set; }

        private Employee()
        {
        }

        public Employee(EmployeeFirstName firstName,
            EmployeeMiddleName middleName,
            EmployeeLastName lastName,
            BirthDay birthDay,
            HiringDate hiringDate,
            Email email)
        {
            BirthDay = birthDay;
            Email = email;
            MiddleName = middleName;
            LastName = lastName;
            FirstName = firstName;
            HiringDate = hiringDate;
            IsDismissed = false;
        }

        /// <summary>
        /// Уволить сотрудника
        /// </summary>
        public void Dismiss()
        {
            if(IsDismissed) return;
            
            IsDismissed = true;
            var employeeWasDismissedDomainEvent = new EmployeeWasDismissedDomainEvent();
            AddDomainEvent(employeeWasDismissedDomainEvent);
        }

        /// <summary>
        /// Проверить стажа сотрудника на достижение определённого срока
        /// </summary>
        /// <param name="date">Текущая дата</param>
        /// <exception cref="EmployeeIsDismissedException">Сотрудник уволен</exception>
        public void CheckWorkExperience(DateTime date)
        {
            if (HiringDate.Value.CompareTo(date) > 0) throw new ArgumentException(nameof(date));
            if (IsDismissed) throw new EmployeeIsDismissedException("Employee was dismissed");
            var calcWorkExperience =
                (date.Year * 12 + date.Month) - (HiringDate.Value.Year * 12 + HiringDate.Value.Month);
            switch (calcWorkExperience)
            {
                case 0:
                    var employeeWasHiredDomainEvent = new EmployeeWasHiredDomainEvent();
                    AddDomainEvent(employeeWasHiredDomainEvent);
                    break;
                case 4:
                    var employeeFinishedProbationPeriodDomainEvent = new EmployeeFinishedProbationPeriodDomainEvent();
                    AddDomainEvent(employeeFinishedProbationPeriodDomainEvent);
                    break;
                case 61:
                    var employeeBecameVeteranDomainEvent = new EmployeeBecameVeteranDomainEvent();
                    AddDomainEvent(employeeBecameVeteranDomainEvent);
                    break;
            }
        }
    }
}