using System;
using System.Linq;

using GradeBook.Enums;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        private readonly int minClassSize;

        public RankedGradeBook(string name) : base(name)
        {
            Type = GradeBookType.Ranked;
            minClassSize = 5;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            // Ranked-grading requires a minimum of 5 students to work)
            if (Students.Count < minClassSize) { throw new InvalidOperationException();}

            /*'d recommend figuring out how many students it takes to drop a letter grade 
            (20% of the total number of students) X, put all the average grades in order, 
            then figure out where the input grade would fit in the series of grades. 
            Every X students with higher grades than the input grade knocks the letter grade
            down until you reach F.
            */

            Students.Sort( (s2, s1) => s1.AverageGrade.CompareTo(s2.AverageGrade) );

            int A_GradeStudents = (int)( (Students.Count * 1.0) - (Students.Count * 0.8));
            int B_GradeStudents_TopRange = (int)( (Students.Count * 0.8) - (Students.Count * 0.6)) + A_GradeStudents;
            int C_GradeStudents_TopRange = (int)( (Students.Count * 0.6) - (Students.Count * 0.4)) + B_GradeStudents_TopRange;
            int D_GradeStudents_TopRange = (int)( (Students.Count * 0.4) - (Students.Count * 0.2)) + C_GradeStudents_TopRange;
            int F_GradeStudents = (int)( (Students.Count * 0.2)) + D_GradeStudents_TopRange;
            
            if ( (averageGrade <= Students[A_GradeStudents-1].AverageGrade) &&  
                 (averageGrade >  Students[B_GradeStudents_TopRange-1].AverageGrade)) return 'A';
            
            if ( (averageGrade <= Students[B_GradeStudents_TopRange-1].AverageGrade) &&  
                 (averageGrade >  Students[C_GradeStudents_TopRange-1].AverageGrade)) return 'B';

            if ( (averageGrade <= Students[C_GradeStudents_TopRange-1].AverageGrade) &&  
                 (averageGrade >  Students[D_GradeStudents_TopRange-1].AverageGrade)) return 'C';

            if ( (averageGrade <= Students[D_GradeStudents_TopRange-1].AverageGrade) &&  
                 (averageGrade >  Students[F_GradeStudents-1].AverageGrade)) return 'D';

            return 'F';
        }
    }
}
