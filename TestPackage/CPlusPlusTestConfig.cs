using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Serialization;
using EnvDTE;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [Serializable]
    [XmlRoot("Configuration")]
    public class CPlusPlusTestConfig
    {
        private List<ConfiguredProject> _projects;
        private string _configFilePath;

        [XmlIgnore]
        public string FilePath
        {
            get { return _configFilePath; }
            private set { _configFilePath = value; }
        }

        /*[XmlElement("Project")]
         *
        public List<ConfiguredProject> SavedProjects
        {
            get { return  _projects == null ? new List<ConfiguredProject>() : _projects.FindAll(p =>p.Saved); }
            set { _projects = value; }
        }*/

        [XmlElement("Project")]
        public List<ConfiguredProject> Projects
        {
            get { return _projects; }
            set { _projects = value; }

        }

        private CPlusPlusTestConfig()
        {
            //_projects = new List<ConfiguredProject>();
        }

        private CPlusPlusTestConfig(string filepath)
        {
            _configFilePath = filepath;
            _projects = new List<ConfiguredProject>();
        }

        public static CPlusPlusTestConfig Open(Solution vsSolution)
        {
            Contract.Requires(vsSolution != null);
            Contract.Ensures(Contract.Result<CPlusPlusTestConfig>() != null);

            FileInfo solutionInfo = new FileInfo(vsSolution.FullName);
            string fullConfigPath = solutionInfo.Directory + "\\" + solutionInfo.Name + ".CPlusPlusTest.user";

            if (File.Exists(fullConfigPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CPlusPlusTestConfig));
                using (FileStream file = File.OpenRead(fullConfigPath))
                {
                    CPlusPlusTestConfig config = (CPlusPlusTestConfig)serializer.Deserialize(file);
                    if (config == null)
                        throw new IOException("Could not deserialize:" + file);
                    config.FilePath = fullConfigPath;
                    return config;
                }

            }
            return new CPlusPlusTestConfig(fullConfigPath);
        }

        public void Save()
        {
            Contract.Requires(Projects != null);
            Contract.Assert(_configFilePath != null);
            if (_configFilePath == null)
                throw new NullReferenceException("Configuration is invaild");

            if (File.Exists(_configFilePath))
            {
                if (Projects.Count < 1)
                {
                    File.Delete(_configFilePath);
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CPlusPlusTestConfig));
                    using (FileStream file = File.OpenWrite(_configFilePath))
                    {
                        serializer.Serialize(file, this);
                    }

                    foreach (var configuredProject in Projects)
                        if (configuredProject != null) configuredProject.Dirty = false;
                }
            }
            else
                if (Projects.Count > 0)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CPlusPlusTestConfig));
                    using (FileStream file = File.Create(_configFilePath))
                    {
                        serializer.Serialize(file, this);
                    }
                    foreach (var configuredProject in Projects)
                        configuredProject.Dirty = false;
                }
        }

        public ConfiguredProject GetConfiguration(Project project)
        {
            Contract.Requires(project != null);
            Contract.Ensures(Contract.Result<ConfiguredProject>() != null);
            Contract.Assert(this._projects != null);
            if (_projects == null)
            {
                throw new NullReferenceException("Projects is Null!");
            }
            ConfiguredProject configuredProject = _projects.Find(p => p.Name == project.Name);

            if (configuredProject == null)
            {
                configuredProject = new ConfiguredProject(false, project);
                _projects.Add(configuredProject);
            }
            else
                configuredProject.BelongsTo = project;

            return configuredProject;
        }
    }

    [Serializable]
    public class ConfiguredProject
    {
        private Project _underlyingProject;

        public ConfiguredProject()
        { }

        public ConfiguredProject(bool saved, Project underlyingProject)
        {
            Saved = saved;
            _underlyingProject = underlyingProject;
        }

        [XmlAttribute("name")]
        public string Name;

        public string ListTestCommand = Resources.DefaultGTestList;
        public string RunTestCommand = Resources.DefaultGTestRun;
        public string TestExe;

        private bool _dirty;
        [XmlIgnore]
        public bool Saved = true;

        [XmlIgnore]
        public bool Dirty
        {
            get { return _dirty; }
            set { _dirty = value; }
        }

        [XmlIgnore]
        public Project BelongsTo
        {
            get { return _underlyingProject; }
            set { _underlyingProject = value; }
        }
    }
}
