using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    /// <summary>
    /// Interface of abstract stateful service
    /// </summary>
    public interface IStatefulService
    {
        /// <summary>
        /// Saves self state somewhere
        /// </summary>
        void SaveState();

        /// <summary>
        /// Loads saved state of service
        /// </summary>
        void LoadSavedState();
    }
}
