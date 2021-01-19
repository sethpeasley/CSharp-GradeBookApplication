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
            double netAverageGrades = 0.0;
        //     foreach (Student st in Students)
        //     {
        //         netAverageGrades += st.AverageGrade;
        //     }

        //     double averageTotal = netAverageGrades / Students.Count;
        //     Console.WriteLine($"netAverageGrades: {netAverageGrades}, Students.Count: {Students.Count}, Averagetotal: {averageTotal}, averageGrade {averageGrade}");

        //     if (averageTotal >= averageGrade * 0.8) { return 'A'; }
        //     if (averageTotal >= averageGrade * 0.6) { return 'B'; }
        //     if (averageTotal >= averageGrade * 0.4) { return 'C'; }
        //     if (averageTotal >= averageGrade * 0.2) { return 'D'; }
        //    // if (averageGrade >= averageGrade * 0.6) { return 'F'; }

            Students.Sort( (s2, s1) => s1.AverageGrade.CompareTo(s2.AverageGrade) );

/*
            Dictionary<char, int> studentDist = new Dictionary<char, int>();
            studentDist['A'] = (int)( (Students.Count * 1.0) - (Students.Count * 0.8));
            studentDist['B'] = (int)( (Students.Count * 0.8) - (Students.Count * 0.6));
            studentDist['C'] = (int)( (Students.Count * 0.6) - (Students.Count * 0.4));
            studentDist['D'] = (int)( (Students.Count * 0.4) - (Students.Count * 0.2));
            studentDist['F'] = (int)( (Students.Count * 0.2));

            
            if ( (averageGrade <= Students[studentDist['A']-1].AverageGrade) &&  
                 (averageGrade >  Students[studentDist['A']+studentDist['B']-1].AverageGrade)) return 'A';
            
            if ( (averageGrade <= Students[studentDist['A']+studentDist['B']-1].AverageGrade) &&  
                 (averageGrade >  Students[studentDist['A']+studentDist['B']+studentDist['C']-1].AverageGrade)) 
                 {return 'B';}
            
            if ( (averageGrade <= Students[studentDist['A']+studentDist['B']+studentDist['C']-1].AverageGrade) &&  
                 (averageGrade >  Students[studentDist['A']+studentDist['B']+studentDist['C']+studentDist['D']-1].AverageGrade)) return 'C';
            
            if ( (averageGrade <= Students[studentDist['A']+studentDist['B']+studentDist['C']+studentDist['D']-1].AverageGrade) &&  
                 (averageGrade >  Students[studentDist['A']+studentDist['B']+studentDist['C']+studentDist['D']+studentDist['F']-1].AverageGrade)) return 'D';
            return 'F';
*/

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
