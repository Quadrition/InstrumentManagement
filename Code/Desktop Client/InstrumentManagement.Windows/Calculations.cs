namespace InstrumentManagement.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A calculations class containing a basic standard deviation calculation
    /// </summary>
    public static class Calculations
    {
        /// <summary>
        /// Calculates a standard deviation of the <paramref name="values"/>
        /// </summary>
        /// <param name="values">A list of double values for the calculation</param>
        /// <returns>A standard deviation of the <paramref name="values"/></returns>
        public static double StandardDeviation(IEnumerable<double> values)
        {
            double avg = values.Average();

            double sum = values.Sum(value => Math.Pow(value - avg, 2));

            return Math.Sqrt(sum / (values.Count() - 1));
        }
    }
}
