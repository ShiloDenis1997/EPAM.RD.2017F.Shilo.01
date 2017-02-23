using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Exceptions.StatefulService;

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
        /// <exception cref="CannotSaveStateException"> Throws if cannot
        /// save state for some reasons</exception>
        /// <exception cref="StatefulServiceException"> Throws when there is
        /// some general problem with save state functionallity</exception>
        void SaveState();

        /// <summary>
        /// Loads saved state of service
        /// </summary>
        /// <exception cref="CannotLoadStateException"> Throws if cannot
        /// load state for some reasons</exception>
        ///  <exception cref="StatefulServiceException"> Throws when there is
        /// some general problem with save state functionallity</exception>
        void LoadSavedState();
    }
}
