using Curly_TWM.Domain.Entities;
using Curly_TWM.Domain.Interfaces;
using Curly_TWM.Infrastructure.DbaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Curly_TWM.Infrastructure.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly TWMDB _context;
        public UnitOfWork()
        {
            this._context = new TWMDB();
        }

        //======================================Partners=====================

        private IPartnersRepository _Partners; //TABLE _NAME
        public IPartnersRepository Partners   //TABLE NAME
        {
            get
            {
                if (this._Partners == null)
                {
                    this._Partners = new PartnersRepository(_context);
                }
                return this._Partners;
            }
        }

        //===========================================================
   
        //======================================RiskList=====================

        private IRiskListRepository _RiskList; //TABLE _NAME
        public IRiskListRepository RiskList   //TABLE NAME
        {
            get
            {
                if (this._RiskList == null)
                {
                    this._RiskList = new RiskListRepository(_context);
                }
                return this._RiskList;
            }
        }

        //===========================================================
   
        //======================================OperationalIndicators=====================

        private IOperationalIndicatorsRepository _OperationalIndicators; //TABLE _NAME
        public IOperationalIndicatorsRepository OperationalIndicators   //TABLE NAME
        {
            get
            {
                if (this._OperationalIndicators == null)
                {
                    this._OperationalIndicators = new OperationalIndicatorsRepository(_context);
                }
                return this._OperationalIndicators;
            }
        }

        //===========================================================
   

        //======================================Team_Tasks=====================

        private ITeam_TasksRepository _Team_Tasks; //TABLE _NAME
        public ITeam_TasksRepository Team_Tasks   //TABLE NAME
        {
            get
            {
                if (this._Team_Tasks == null)
                {
                    this._Team_Tasks = new Team_TasksRepository(_context);
                }
                return this._Team_Tasks;
            }
        }

        //===========================================================
   
        //======================================Teams=====================

        private ITeamsRepository _Teams; //TABLE _NAME
        public ITeamsRepository Teams   //TABLE NAME
        {
            get
            {
                if (this._Teams == null)
                {
                    this._Teams = new TeamsRepository(_context);
                }
                return this._Teams;
            }
        }

        //===========================================================
   

        //======================================Initiatives=====================

        private IInitiativesRepository _Initiatives; //TABLE _NAME
        public IInitiativesRepository Initiatives   //TABLE NAME
        {
            get
            {
                if (this._Initiatives == null)
                {
                    this._Initiatives = new InitiativesRepository(_context);
                }
                return this._Initiatives;
            }
        }

        //===========================================================
   

        //======================================Strategic_Targets=====================

        private IStrategic_TargetsRepository _Strategic_Targets; //TABLE _NAME
        public IStrategic_TargetsRepository Strategic_Targets   //TABLE NAME
        {
            get
            {
                if (this._Strategic_Targets == null)
                {
                    this._Strategic_Targets = new Strategic_TargetsRepository(_context);
                }
                return this._Strategic_Targets;
            }
        }

        //===========================================================
   
        //======================================Strategic_Plans=====================

        private IStrategic_PlansRepository _Strategic_Plans; //TABLE _NAME
        public IStrategic_PlansRepository Strategic_Plans   //TABLE NAME
        {
            get
            {
                if (this._Strategic_Plans == null)
                {
                    this._Strategic_Plans = new Strategic_PlansRepository(_context);
                }
                return this._Strategic_Plans;
            }
        }

        //===========================================================
        

        //======================================Jobs=====================

        private IJobsRepository _Jobs; //TABLE _NAME
        public IJobsRepository Jobs   //TABLE NAME
        {
            get
            {
                if (this._Jobs == null)
                {
                    this._Jobs = new JobsRepository(_context);
                }
                return this._Jobs;
            }
        }

        //===========================================================
        

        //======================================Branchs=====================

        private IBranchsRepository _Branchs; //TABLE _NAME
        public IBranchsRepository Branchs   //TABLE NAME
        {
            get
            {
                if (this._Branchs == null)
                {
                    this._Branchs = new BranchsRepository(_context);
                }
                return this._Branchs;
            }
        }

        //===========================================================
        

        //======================================Sections=====================

        private ISectionsRepository _Sections; //TABLE _NAME
        public ISectionsRepository Sections   //TABLE NAME
        {
            get
            {
                if (this._Sections == null)
                {
                    this._Sections = new SectionsRepository(_context);
                }
                return this._Sections;
            }
        }

        //===========================================================
        
        //======================================Departments=====================

        private IDepartmentsRepository _Departments; //TABLE _NAME
        public IDepartmentsRepository Departments   //TABLE NAME
        {
            get
            {
                if (this._Departments == null)
                {
                    this._Departments = new DepartmentsRepository(_context);
                }
                return this._Departments;
            }
        }

        //===========================================================
        

        //======================================MainDepartments=====================

        private IMainDepartmentsRepository _MainDepartments; //TABLE _NAME
        public IMainDepartmentsRepository MainDepartments   //TABLE NAME
        {
            get
            {
                if (this._MainDepartments == null)
                {
                    this._MainDepartments = new MainDepartmentsRepository(_context);
                }
                return this._MainDepartments;
            }
        }

        //===========================================================
        
        //======================================emp_training=====================

        private Iemp_trainingRepository _emp_training; //TABLE _NAME
        public Iemp_trainingRepository emp_training   //TABLE NAME
        {
            get
            {
                if (this._emp_training == null)
                {
                    this._emp_training = new emp_trainingRepository(_context);
                }
                return this._emp_training;
            }
        }

        //===========================================================

        //========================================emp_main===================

        private IEmp_mainRepository _emp_main; //TABLE _NAME
        public IEmp_mainRepository emp_main   //TABLE NAME
        {
            get
            {
                if (this._emp_main == null)
                {
                    this._emp_main = new Emp_mainRepository(_context);
                }
                return this._emp_main;
            }
        }

        //===========================================================

        //===========$$$$$$$$$$GenericRepo$$$$$$$$$$$$===========
        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public IGenericRepository<T> GenericRepos<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(_context);
            repositories.Add(typeof(T), repo);
            return repo;
        }
        //===========$$$$$$$$$$GenericRepo$$$$$$$$$$$$============


        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}