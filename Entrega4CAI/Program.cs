using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Entrega4CAI
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathBase = Directory.GetCurrentDirectory();


            // Carga inicial de archivos con datos

            List<Alumno> alumnos = new List<Alumno>();
            List<Carrera> carreras = new List<Carrera>();

            System.IO.StreamReader reader = new StreamReader(File.OpenRead(@pathBase+ "/Data/Contador.txt"));
            carreras.Add(cargarCarrera(reader, "Contador Publico"));
            reader = new StreamReader(File.OpenRead(@pathBase + "/Data/Actuario.txt"));
            carreras.Add(cargarCarrera(reader, "Actuario"));
            reader = new System.IO.StreamReader(File.OpenRead(@pathBase + "/Data/Administrador.txt"));
            carreras.Add(cargarCarrera(reader, "Administracion"));
            reader = new System.IO.StreamReader(File.OpenRead(@pathBase + "/Data/Sistemas.txt"));
            carreras.Add(cargarCarrera(reader, "Licenciatura en sistemas de informacion"));

            reader = new System.IO.StreamReader(File.OpenRead(@pathBase + "/Data/Legajos.txt"));
            alumnos = cargarAlumnos(reader);

            reader = new System.IO.StreamReader(File.OpenRead(@pathBase + "/Data/Analitico.txt"));
            alumnos = cargarAnaliticos(reader, alumnos);

            reader = new System.IO.StreamReader(File.OpenRead(@pathBase + "/Data/OfertaAcademica.txt"));
            Oferta oferta = cargarOferta(reader);
            oferta.Activa = true;

            /////////////////////////////////////////////////////////////////////////



            Console.WriteLine("\n Bienvenido al sistema de inscripciones \n");
            Console.WriteLine("Ingrese su numero de registro: \n");
            String registro = Console.ReadLine();
            Console.WriteLine("\nIngrese su contraseña: \n");
            String password = Console.ReadLine();

            Alumno alumno = ValidarLogin(registro, password, alumnos);
            while (alumno == null) {
                Console.WriteLine("\n ¡ERROR! Registro y/o contraseña incorrectos. \n");
                Console.WriteLine("Ingrese su numero de registro: \n");
                registro = Console.ReadLine();
                Console.WriteLine("\nIngrese su contraseña: \n");
                password = Console.ReadLine();
                alumno = ValidarLogin(registro, password, alumnos);
            }

            Console.WriteLine("\nInicio de sesion exitoso.\n\nAlumno: " + alumno.Nombre + "\n");

            int rtdo = MostrarMenu(oferta.Activa);
            Boolean run = true;

            while (run)
            {
                switch (rtdo)
                {
                    case 1:

                        foreach (Curso c in oferta.OfertaAc)
                        {
                            Console.WriteLine("\nMateria: " + c.Materia + "(" + c.Code_materia + ").\n");
                            Console.WriteLine("Profesor: " + c.Profesor + ".\n");
                            Console.WriteLine("Codigo del curso: " + c.Code + ".\n");
                            Console.WriteLine("Catedra: " + c.Catedra + ".\n");
                            Console.WriteLine("Horario: " + c.HoraInicio + "-" + c.HoraInicio + 2 + ".\n");
                        }
                        rtdo = MostrarMenu(oferta.Activa);
                        Console.WriteLine("\nPresione una tecla para continuar.\n");
                        break;
                    case 2:

                        reader = new System.IO.StreamReader(File.OpenRead(@pathBase + "/Data/Inscripciones.txt"));
                        if (!validarOpcionInscripcion(reader, alumno.Registro)) {

                            Console.WriteLine("\nYa se registraron incripciones para este alumno.\n Presione cualquier tecla para salir.\n");
                            run = false;
                            break;
                        }
            

                        int cantHabilitadas = declaracionMaterias(alumno);

                        int cantInscriptas = 0;

                        alumno= elegirCarrera(carreras, alumno);

                        Carrera eleccionCarrera = alumno.Carrera;

                        bool seguir = true;

                        while (seguir)
                        {

                            Console.WriteLine("\nIngrese el codigo de la materia a inscribirse:\n");
                            validarInscripcion(Validar(Console.ReadLine()), oferta, alumno);
                            cantInscriptas++;
                            bool next = true;
                            while (next && cantInscriptas < 4)
                            {
                                Console.WriteLine("\n¿Desea insribirse en otra materia?\n");
                                Console.WriteLine("S - Si\n");
                                Console.WriteLine("N -No\n");

                                if (ValidarYN(Console.ReadLine()))
                                {
                                    Console.WriteLine("\nIngrese el codigo de la materia a inscribirse:\n");
                                    validarInscripcion(Validar(Console.ReadLine()), oferta, alumno);
                                    cantInscriptas++;
                                }
                                else
                                {
                                    next = false;
                                    rtdo = MostrarMenu(oferta.Activa);
                                    seguir = false;
                                    break;
                                }
                                break;
                            }
                            if (cantInscriptas == cantHabilitadas)
                            {
                                Console.WriteLine("\nYa se incribió en "+cantHabilitadas+" materias.\n");
                                break;
                            }

                        }

                        rtdo = MostrarMenu(oferta.Activa);
                        Console.WriteLine("\nPresione una tecla para continuar.\n");
                        break;

                    case 3:
                        Console.WriteLine("Presione cualquier tecla para salir.\n");
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Opción erronea. Intente de nuevo.\n");
                        rtdo = MostrarMenu(oferta.Activa);
                        break;
                }
                Console.ReadKey();
            }



        }


        public static List<Alumno> cargarAnaliticos(System.IO.StreamReader reader, List<Alumno> alumnos) {
          
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                foreach (Alumno a in alumnos) {
                    if (int.Parse(values[0])==a.Registro) {
                        a.MateriasAprobadas.Add(int.Parse(values[1]));
                        break;
                    }
                }
            }
            return alumnos;

        }

        public static void exportInscripciones(Alumno alumno)
        {
            var pathBase = Directory.GetCurrentDirectory();
            string path = pathBase+"/Data/Inscripciones.txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (Inscripcion i in alumno.MateriasInscriptas)
                {
                    if (i.Alternativo != null)
                    {
                        sw.WriteLine(alumno.Registro + ";" + i.Original.Code + ";" + i.Alternativo.Code);
                    }
                    else {
                        sw.WriteLine(alumno.Registro + ";" + i.Original.Code);
                    }
                }
            }

        }

        public static Carrera cargarCarrera(System.IO.StreamReader reader, String name) {

            Carrera c = new Carrera(name);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                int[] prequisitos = {(int.Parse(values[3])), (int.Parse(values[4])), (int.Parse(values[5])), (int.Parse(values[6])), (int.Parse(values[7])) };
             
                c.Plan.Add(new Materia(int.Parse(values[0]), values[1], int.Parse(values[2]), prequisitos));
            }
            return c;
        }

        public static Alumno elegirCarrera(List<Carrera> carreras, Alumno a)
        {
            int code = 1;
            Carrera choice = null;
            Console.WriteLine("\n¿Que carrera desea cursar?\n");
            foreach (Carrera c in carreras)
            {
                c.Code = code;
                Console.WriteLine(c.Nombre + "(" + code + ").\n");
                code++;
            }
            Console.WriteLine("Ingrese el codigo de la carrera elegida: \n");
            int opcion = Validar(Console.ReadLine());
            bool ok = false;

            while (!ok)
            {
                int count = 0;
                foreach (Carrera c in carreras)
                {
                    count++;
                    if (c.Code == opcion)
                    {
                        ok = true;
                        choice = c;
                        a.Carrera = c;

                        foreach (Materia m in a.Carrera.Plan) {
                            int contador = 0;
                            for (int i = 0; i < 5; i++) {
                                
                            if (a.MateriasAprobadas.Contains(m.RequisitosPrevio[i]) || m.RequisitosPrevio[i] == 0)
                              {
                                    contador++;                              
                               }
                            }
                            if (contador == 5) {
                            a.MateriasDispo.Add(m.Code);
                            }
                        }
                     return a;
                    }
                }
                if (carreras.Count == count && choice == null ) {
                  Console.WriteLine("\nCodigo erroneo. Intente de nuevo: \n");
                  opcion = Validar(Console.ReadLine());
                }
              
            }

            return a;
        }

        public static Boolean validarOpcionInscripcion(System.IO.StreamReader reader, int registro)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                if (int.Parse(values[0]) == registro) {

                    return false;
                    break;
                }
            }
            return true;
        }
        public static Oferta cargarOferta(System.IO.StreamReader reader)
        {
            Oferta o = new Oferta();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                o.OfertaAc.Add(new Curso(int.Parse(values[0]), values[1], int.Parse(values[2]),  values[3], values[4], int.Parse(values[5] )) );
        
               
            }
            return o;
        }

        public static List<Alumno> cargarAlumnos(System.IO.StreamReader reader)
        {
           
            List<Alumno> alumnos = new List<Alumno>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                alumnos.Add(new Alumno(values[0], values[1], int.Parse(values[2])));
            }
            return alumnos;
        }

        public static Alumno ValidarLogin(String registro, String pass, List<Alumno> alumnos)
        {
            foreach (Alumno a in alumnos) {
                if (a.Registro == int.Parse(registro)) {
                    if (a.Password == pass)
                    {
                        return a;
                    }
                    else {
                        return null;
                    }
                    break;
                }
            }
            return null;

        }

        public static int declaracionMaterias(Alumno alumno) {

            Console.WriteLine("\n Antes de comenzar, ¿Usted se encuentra en las ultimas 4 materias?\n");
            Console.WriteLine("S - Si\n");
            Console.WriteLine("N - No\n");
            bool ultimas = ValidarYN(Console.ReadLine());
            if (ultimas)
            {
                alumno.UltimasMaterias = true;
                return 4;
            }
            else {
                return 3;
            }

        }

        public static int Validar(String input)
        {
            Boolean opcionOk = int.TryParse(input, out int rtdo);
            while (!opcionOk || rtdo < 0)
            {
                Console.WriteLine("La opcion no es valida. Intente de nuevo.\n");
                opcionOk = int.TryParse(Console.ReadLine(), out rtdo);
            }
            return rtdo;
        }

        public static Boolean ValidarYN(String input)
        {
            input = input.ToUpper();
            Boolean seguir = false;

            while (!input.Equals("S") && !input.Equals("N"))
            {
                Console.WriteLine("La opcion no es valida. Intente de nuevo.\n");
                input = Console.ReadLine().ToUpper();
            }
            if (input.ToUpper().Equals("S"))
            {
                seguir = true;
            }
            if (input.ToUpper().Equals("N"))
            {
                seguir = false;
            }
            return seguir;
        }

        public static int MostrarMenu(bool activaOferta)
        {
            if (activaOferta)
            {
                Console.WriteLine("\n Menú: \n");
                Console.WriteLine("1- Mostrar oferta.\n");
                Console.WriteLine("2- Inscribirse.\n");
                Console.WriteLine("3- Salir.\n");
                String input = Console.ReadLine();
                return Validar(input);
            }
            else {
                Console.WriteLine("\n No hay incripciones activas. Intente en otra fecha. \n");
                return 3;
            }
        }

        public static void validarInscripcion(int code, Oferta of, Alumno a)
        {
            bool seguir = true;
            bool ok = false;
            int count = 0;
            code = ValidarMateria(code, a.Carrera);
            code = validarRepetida(code, a);

            while (seguir)
            {
                foreach (int d in a.MateriasDispo)
                {
                    count++;
                    if (d == code|| a.UltimasMaterias)
                    {
                        Console.WriteLine("\nIngrese el codigo del curso original:\n");
                        Curso orig = ValidarCurso(Validar(Console.ReadLine()), of, code);

                        Console.WriteLine("\n¿Desea cargar un curso alternativo?\n");
                        Console.WriteLine("S - Si\n");
                        Console.WriteLine("N -No\n");

                        if (ValidarYN(Console.ReadLine()))
                        {

                            Console.WriteLine("\nIngrese el codigo del curso alternativo:\n");
                            Curso alt = ValidarCurso(Validar(Console.ReadLine()), of,code);
                            Inscripcion insc = new Inscripcion(orig, alt);
                            a.MateriasInscriptas.Add(insc);
                            exportInscripciones(a);
                            ok = true;
                            seguir = false;
                        }
                        else
                        {                 
                            Inscripcion insc = new Inscripcion(orig, null);
                            a.MateriasInscriptas.Add(insc);
                            exportInscripciones(a);
                            ok = true;
                            seguir = false;
                        }
                        break;
                    }
                }
                if (count == a.MateriasDispo.Count && !ok)
                {
                    Console.WriteLine("\nTodavia no puede cursar esa materia. Intente con otra.\n");
                    code = Validar(Console.ReadLine());
                    validarInscripcion(code, of, a);
                }
            }
        }

  

        public static Curso ValidarCurso(int code, Oferta of, int code_materia)
        {
            int count = 0;
            bool seguir = true;
            Curso curso = null;
            while (seguir)
            {
                foreach (Curso c in of.OfertaAc)
                {
                    count++;
                    if (c.Code == code && c.Code_materia == code_materia)
                    {
                        curso = c;
                        seguir = false;
                        Console.WriteLine("\nCurso cargado con exito.\n");
                        count = 0;
                        return curso;
                    }
                }
                if (count == of.OfertaAc.Count && curso == null)
                {
                    Console.WriteLine("\nCodigo erroneo. Intente de nuevo.\n");
                    count = 0;
                    code = Validar(Console.ReadLine());
                }
            }
            return curso;
        }

        public static int ValidarMateria(int code, Carrera carrera) {

            int count = 0;
            bool ok = false;

            foreach (Materia m in carrera.Plan) {
                count++;
                if (m.Code == code) {
                    ok = true;
                    break;
                }
            }
            if (count == carrera.Plan.Count && !ok) {
                Console.WriteLine("Codigo de materia erroneo. Intente de nuevo.\n");
                code = Validar(Console.ReadLine());
                ValidarMateria(code, carrera);
            }

            return code;
        }

        public static int validarRepetida(int code, Alumno a) {

            bool ok = false;    
            int insc = 0;
            while (!ok) {

                if(a.MateriasAprobadas.Contains(code)) {
                        Console.WriteLine("\nEsa materia ya está aprobada. Intente con otra.\n");
                        code = Validar(Console.ReadLine());
                }

                foreach (Inscripcion i in a.MateriasInscriptas) {
                    insc++;
                    if (i.Alternativo != (null))
                    {
                        if (i.Original.Code == code || i.Alternativo.Code == code)
                        {
                            Console.WriteLine("\nYa está inscripto a esa materia. Intente con otra.\n");
                            code = Validar(Console.ReadLine());
                            break;
                        }
                    }
                    else {

                        if (i.Original.Code == code)
                        {
                            Console.WriteLine("\nYa está inscripto a esa materia. Intente con otra.\n");
                            code = Validar(Console.ReadLine());
                            break;
                        }
                    }      
                }
                if(insc == a.MateriasInscriptas.Count)
                {
                    ok = true;
                    break;
                }
            }
            return code;
        }
    }
        

        class Alumno {

        private String nombre;
        private String password;
        private int registro;
        private Carrera carrera;
        private List<int> materiasAprobadas = new List<int>();
        private List<int> materiasDisponibles = new List<int>();
        private List<Inscripcion> materiasInscriptas = new List<Inscripcion>();
        private bool ultimasMaterias = false;

        public Alumno(string nombre, string password, int registro)
        {
            this.nombre = nombre;
            this.password = password;
            this.registro = registro;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public int Registro { get => registro; set => registro = value; }
        public List<int> MateriasAprobadas { get => materiasAprobadas; set => materiasAprobadas = value; }
        public List<Inscripcion> MateriasInscriptas { get => materiasInscriptas; set => materiasInscriptas = value; }
        public Carrera Carrera { get => carrera; set => carrera = value; }

        public List<int> MateriasDispo{ get => materiasDisponibles; set => materiasDisponibles = value; }
        public bool UltimasMaterias { get => ultimasMaterias; set => ultimasMaterias = value; }
    }

    class Materia {

        private String nombre;
        private int code;
        private List<Materia> materiasSiguientes = new List<Materia>();
        private int cargaHoraria;
        private int[] code_correlativa = new int[5];

        public Materia(int code, String nombre, int carga, int[] code_correlativa)
        {
            this.code = code;
            this.nombre = nombre;
            this.cargaHoraria = carga;
            this.code_correlativa = code_correlativa;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public int Code { get => code; set => code = value; }
        public List<Materia> MateriasSiguientes { get => materiasSiguientes; set => materiasSiguientes = value; }
        public int[] RequisitosPrevio { get => code_correlativa; set => code_correlativa = value; }
    }

    class Curso {

        private string materia;
        private int code_materia;
        private int code;
        private String profesor;
        private String catedra;
        private int horaInicio;
        public Curso(int code, string materia, int code_materia, string profesor, string catedra, int horaInicio)
        {
            this.materia = materia;
            this.Code = code;
            this.profesor = profesor;
            this.Code_materia = code_materia;
            this.Catedra = catedra;
            this.HoraInicio = horaInicio;
        }

        public string Materia { get => materia; set => materia = value; }
        public string Profesor { get => profesor; set => profesor = value; }
        public int Code { get => code; set => code = value; }
        public int Code_materia { get => code_materia; set => code_materia = value; }
        public string Catedra { get => catedra; set => catedra = value; }
        public int HoraInicio { get => horaInicio; set => horaInicio = value; }
    }

    class Carrera {

        private int code;
        private String nombre;
        private List<Materia> plan = new List<Materia>();

        public Carrera()
        {
        }

        public Carrera(string nombre)
        {
            this.nombre = nombre;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Materia> Plan { get => plan; set => plan = value; }
        public int Code { get => code; set => code = value; }
    }

    class Profesor {

        private int dni;
        private String nombre;
        private List<Curso> materias;

        public List<Curso> Materias { get => materias; set => materias = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int Dni { get => dni; set => dni = value; }
    }


    class Oferta {
    
    private List<Curso> ofertaAc = new List<Curso>();
        private bool activa;

        public List<Curso> OfertaAc { get => ofertaAc; set => ofertaAc = value; }
        public bool Activa { get => activa; set => activa = value; }
    }

    class Inscripcion {

        private Curso original;
        private Curso alternativo;

        public Inscripcion(Curso original, Curso alternativo)
        {
            this.original = original;
            this.alternativo = alternativo;

        }

        public Curso Original { get => original; set => original = value; }
        public Curso Alternativo { get => alternativo; set => alternativo = value; }
    }
}
