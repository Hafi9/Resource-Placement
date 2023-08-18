﻿using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.Repositories;
using API.Utilities.Handlers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

        }

        public int CountEmployee()
        {
            var data = _employeeRepository.GetAll();
            var counter = 0;
            foreach (var item in data)
            {
                counter++;
            }
            return counter;
        }

        public GetCountedGender? CountGender()
        {
            var data = _employeeRepository.GetAll();
            var countfemale = 0;
            var countmale = 0;

            foreach (var item in data)
            {
                if (item.Gender == 0)
                {
                    countfemale++;
                }
                else
                {
                    countmale++;
                }
            }
            return new GetCountedGender
            {
                CountFemale = countfemale,
                CountMale = countmale
            };
        }

        public GetCountedStatus? CountStatus()
        {
            var data = _employeeRepository.GetAll();
            var countidle = 0;
            var countsite = 0;

            foreach (var item in data)
            {
                if (item.Status == 0)
                {
                    countidle++;
                }
                else
                {
                    countsite++;
                }
            }
            return new GetCountedStatus
            {
                CountIdle = countidle,
                CountSite = countsite
            };
        }

        
        public IEnumerable<EmployeeDto> GetAll()
        {
            var employees = _employeeRepository.GetAll();
            if (!employees.Any())
            {
                return Enumerable.Empty<EmployeeDto>(); // Employee is null or not found;
            }

            var employeeDtos = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                employeeDtos.Add((EmployeeDto)employee);
            }

            return employeeDtos; // Employee is found;
        }

        public EmployeeDto? GetByGuid(Guid guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);
            if (employee is null)
            {
                return null; // Employee is null or not found;
            }

            return (EmployeeDto)employee; // Employee is found;
        }

        public EmployeeDto? Create(NewEmployeeDto newEmployeeDto)
        {
            Employee toCreate = newEmployeeDto;
            toCreate.NIK = GenerateHandler.LastNik(_employeeRepository.GetLastNik());

            var employee = _employeeRepository.Create(toCreate);
            if (employee is null)
            {
                return null; // Employee is null or not found;
            }

            return (EmployeeDto)employee; // Employee is found;
        }

        public int Update(UpdateEmployeeDto employeeDto)
        {
            var employee = _employeeRepository.GetByGuid(employeeDto.Guid);
            if (employee is null)
            {
                return -1; // Employee is null or not found;
            }

            Employee toUpdate = employeeDto;
            toUpdate.NIK = employee.NIK;
            toUpdate.CreatedDate = employee.CreatedDate;
            var result = _employeeRepository.Update(toUpdate);

            return result ? 1 // Employee is updated;
                : 0; // Employee failed to update;
        }

        public int Delete(Guid guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);
            if (employee is null)
            {
                return -1; // Employee is null or not found;
            }

            var result = _employeeRepository.Delete(employee);

            return result ? 1 // Employee is deleted;
                : 0; // Employee failed to delete;
        }
    }
}
