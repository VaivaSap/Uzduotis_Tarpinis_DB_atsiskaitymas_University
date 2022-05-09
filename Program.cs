
using Microsoft.EntityFrameworkCore;
using System;


//Funkcionalumai:
//1.Sukurti departamentą ir į jį pridėti studentus, paskaitas(papildomi points jei pridedamos paskaitos jau egzistuojančios duomenų bazėje).
//2. Pridėti studentus/paskaitas į jau egzistuojantį departamentą.
//3. Sukurti paskaitą ir ją priskirti prie departamento.
//4. Sukurti studentą, jį pridėti prie egzistuojančio departamento ir priskirti jam egzistuojančias paskaitas.
//5. Perkelti studentą į kitą departamentą(bonus points jei pakeičiamos ir jo paskaitos).


namespace UniversityStructure
{
    public class Program
    {
        public Program()
        {
        }

        public static void Main(string[] args)
        {
            using (var context = new UniversityContext())

            {
                var departmentId = Guid.Parse("66DE5E00-B45A-4031-8C1E-CDDD01C47BE8");
                DbPrimaryDataCreated(context);
                ShowStudents(context, departmentId); //6 užduotis - - per Department ID atvaizduojami esami studentai

                ShowLectures(context, departmentId); //7 Užduotis - - per Department ID atvaizduojamos paskaitos


                var studentId = Guid.Parse("E1A76FAC-2B8B-439B-810C-EAE50C431D85");
                ShowLecturesOfStudents(context, studentId); //8 užduotis - - atvaizduoti visas studento paskaitas 


                // AddNewLectureToAnExistingDepartment(context, "Ethics", departmentId); 
                // AddNewStudentToAnExistingDepartment(context, "Danas", "Martisius", departmentId);
                AddExistingStudentToExistingLectures(context, studentId, Guid.Parse("B3969361-EA40-4B23-8797-859D66EE5E20"));


            }
        }

        private static void AddANewDepartment(UniversityContext context, string title) //1 užduotis -- sukuriame metodą naujam custom departamentui pridėti
        {
            var addedDepartment = new Department(title);
            context.SaveChanges();

        }

        private static void AddNewLectureToAnExistingDepartment(UniversityContext context, string subject, Guid departmentId) // Metodas pridėti naują paskaitą į esantį departamentą
        {
            var addedLecture = new Lecture(subject);
            context.Lectures.Add(addedLecture);


            var existingDepartment = context.Departments.FirstOrDefault(x => x.Id == departmentId);

            if(existingDepartment == null)
            {
                Console.WriteLine("This department was not found");
                return;
            }

            existingDepartment.Lectures.Add(addedLecture);
            context.SaveChanges();


        }

        private static void AddNewStudentToAnExistingDepartment(UniversityContext context, string name, string surname, Guid departmentId) //Metodas pridėti naują studentą į departamentą
        {
            var addedStudent = new Student(name, surname);
            context.Students.Add(addedStudent);


            var existingDepartment = context.Departments.FirstOrDefault(x => x.Id == departmentId);

            if (existingDepartment == null)
            {
                Console.WriteLine("This department was not found");
                return;
            }

            existingDepartment.Students.Add(addedStudent);
            context.SaveChanges();

        }

        private static void AddExistingStudentToExistingLectures(UniversityContext context, Guid studentId, Guid lectureId) //Kad suradus esamus studentus ir esamas paskaitas būtų galima susieti
        {
            
            
            var existingStudent = context.Students.FirstOrDefault(x => x.Id == studentId);

            if (existingStudent == null)
            {
                Console.WriteLine("The student is not found");
            }



            var existingLecture = context.Lectures.FirstOrDefault(x => x.Id == lectureId);
            if (existingLecture == null)
            {
                Console.WriteLine("This lecture is not found");
            }

            existingLecture.Students.Add(existingStudent);
            context.SaveChanges();
        }


        private static void DbPrimaryDataCreated(UniversityContext context)
        {

            var uniDepartment = context.Departments.FirstOrDefault();
            if (uniDepartment == null)
            {
                var uniDep = new Department("History");
                var uniLec = new Lecture("Hellenistic World");
                var uniLec1 = new Lecture("Women in History");
                uniDep.Lectures.Add(uniLec);
                uniDep.Lectures.Add(uniLec1);
                var historyStudent = new Student("Milda", "Sraige");
                var historyStudent1 = new Student("Antanas", "Mažas");
                uniDep.Students.Add(historyStudent);
                uniDep.Students.Add(historyStudent1);
                uniLec.Students.Add(historyStudent1);
                uniLec.Students.Add(historyStudent);

                context.Departments.Add(uniDep);
                context.SaveChanges();
            }


        }

        private static void ShowStudents(UniversityContext context, Guid departmentId)
        {
            var department = context.Departments.Include("Students").FirstOrDefault(x => x.Id == departmentId);
            if (department == null)
            {
                Console.WriteLine("Department is unknown");
                return;

            }

            var students = department.Students;

            foreach (var student in students)
            {
                Console.WriteLine("Student " + student.Name + " " + student.Surname);
            }
        }

        private static void ShowLectures(UniversityContext context, Guid departmentId)
        {
            var department = context.Departments.Include("Lectures").FirstOrDefault(x => x.Id == departmentId);
            if (department == null)
            {
                Console.WriteLine("Department is unknown");
                return;

            }

            var lectures = department.Lectures;

            foreach (var lecture in lectures)
            {
                Console.WriteLine("Lecture " + lecture.Subject);
            }


        }

        private static void ShowLecturesOfStudents(UniversityContext context, Guid studentId)
        {
            var student = context.Students.Include("Lectures").FirstOrDefault(x => x.Id == studentId);
            if (student == null)
            {
                Console.WriteLine("We cannot find such a student");
                return;

            }

            var lecturesOfStudents = student.Lectures;

            foreach (var lectureOfStudent in lecturesOfStudents)
            {


                Console.WriteLine(value: "Student " + student.Name + " " + student.Surname + " has these lectures: ");
                foreach (var lecture in student.Lectures)
                {
                    Console.WriteLine(lecture.Subject);
                };
            }
        }
    }

}