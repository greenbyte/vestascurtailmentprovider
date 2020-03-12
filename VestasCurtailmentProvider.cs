using System;
using System.Collections.Generic;

namespace Greenbyte.CurtailmentProviderAssignment {

    /// <summary>
    /// Interface for a type that provides turbine curtailment levels over time as well as standard levels.
    /// </summary>
    public interface ITurbineCurtailmentProvider {
        /// <summary>
        /// Get the standard curtailment level for the specified curtailment type.
        /// </summary>
        /// <param name="curtailment">The curtailment to get the standard level for.</param>
        double GetStandardLevel(TurbineCurtailment curtailment);

        /// <summary>
        /// Sets custom curtailment levels for the given curtailment type.
        /// </summary>
        /// <param name="curtailment">The curtailment type to set level for.</param>
        /// <param name="level">The curtailment level to set to.</param>
        void SetCustomLevel(TurbineCurtailment curtailment, double level);

        /// <summary>
        /// Gets the curtailment level that is active for a specific timestamp (in UTC). A custom level is seen as
        /// the active level for a period from its starting timestamp until a new custom level is set. If there
        /// is no custom curtailment level for the specified timestamp, the standard level is used.
        /// </summary>
        /// <param name="curtailment">The curtailment type to get the level of.</param>
        /// <param name="timestamp">The UTC timestamp to get the level of.</param>
        double GetLevel(TurbineCurtailment curtailment, DateTime timestamp);

        /// <summary>
        /// Gets the curtailment level that is active for the current point in time. A custom level is seen as the
        /// active level for a period from its starting timestamp until a new custom level is set. If there is
        /// no custom curtailment level currently active, the standard level is used.
        /// </summary>
        double GetCurrentLevel(TurbineCurtailment curtailment);
    }

    /// <summary>
    /// Enum for different kinds of turbine curtailment. Curtailment occurs when the power plant
    /// is not allowed to output energy and can imply total shutdown or a reduced power output.
    /// </summary>
    public enum TurbineCurtailment {
        Default,
        Noise,
        Bats,
        Shadow,
        BoatAction,
        Technical,
        Grid,
    }

    /// <summary>
    /// Implements a turbine curtailment provider that has standard levels for Vestas turbines.
    /// TODO: We think there are a few bugs in the code below, since the calculations look messed up every now and then.
    ///       There are also a number of things that are yet to be implemented.
    /// TODO: The code could likely be more readable and maintainable somehow.
    /// </summary>
    class VestasCurtailmentProvider : ITurbineCurtailmentProvider {

        public double GetStandardLevel(TurbineCurtailment curtailment) {
            //TODO: please refactor these ugly if statements somehow...
            
            // PLEASE NOTE: THESE ARE THE ACTUAL CURTAILMENT LEVELS THAT SHOULD APPLY FOR VESTAS TURBINES, WE JUST GOT THEM FROM THE OPERATOR!
            // Default = 0%
            // Noise curtailment = 25%
            // Bats curtailment = 15%
            // Shadow curtailment = 10%
            // Boat action curtailment = 5%
            // Technical curtailment = 5%
            // Grid curtailment = 5%

            if (curtailment == TurbineCurtailment.Default) {
                return 0.0;
            }
            if (curtailment == TurbineCurtailment.Noise) {
                return 0.25;
            }
            if (curtailment == TurbineCurtailment.Bats) {
                return 0.15;
            }
            if (curtailment == TurbineCurtailment.Shadow) {
                return 0.1;
            }
            if (curtailment == TurbineCurtailment.BoatAction) {
                return 0.5;
            }
            if (curtailment == TurbineCurtailment.Technical) {
                return 0.5;
            }
            if (curtailment == TurbineCurtailment.Grid) {
                return 0.5;
            }

            return 0.0;
        }

        /// <remarks>
        /// When set, the method saves the curtailment level information for the current timestamp in UTC for later retrieval.
        /// 
        /// Each instance of this object supports a different set of custom levels since we run one thread per customer.
        /// For now we just save it in memory but will use a database later.
        /// </remarks>
        public void SetCustomLevel(TurbineCurtailment curtailment, double level) {
            //TODO: support saving multiple custom levels for different combinations of TurbineCurtailment/DateTime
            //TODO: make sure we never save duplicates, in case of e.g. clock resets, DST etc - overwrite old values if this happens
            _customLevels[curtailment] = Tuple.Create(DateTime.Now, level);
        }

        public double GetLevel(TurbineCurtailment curtailment, DateTime timestamp) {
            //TODO: implement
            throw new NotImplementedException();
        }
        static Dictionary<TurbineCurtailment, Tuple<DateTime, double>> _customLevels = new Dictionary<TurbineCurtailment, Tuple<DateTime, double>>();

        public double GetCurrentLevel(TurbineCurtailment curtailment) {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}
