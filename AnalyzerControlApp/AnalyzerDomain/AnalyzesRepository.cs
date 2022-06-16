using AnalyzerDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain
{
    public class AnalyzesRepository : IDisposable
    {
        public ObservableCollection<AnalysisDescription> Analyzes { get; private set; }

        AnalyzerContext context = new AnalyzerContext();

        public AnalyzesRepository()
        {
            context.Analyses.Load();
            Analyzes = context.Analyses.Local.ToObservableCollection();
        }

        public void Add(AnalysisDescription description)
        {
            context.Analyses.Add(description);
        }

        public void Update(AnalysisDescription analysis)
        {
            context.Entry(analysis).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
