using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega4CAI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Carga inicial
            Materia logica = new Materia(651, "Logica", 4, false);
            Materia comunicaciones = new Materia(655, "Tecnologia de las comunicaciones", 4, true);
            Materia tlya = new Materia(1652, "Teoria de los lenguajes y algoritmos", 6, true);
            Materia dInfI = new Materia(1603, "Derecho Informatico I", 4, false);
            Materia dInfII = new Materia(1604, "Derecho Informatico II", 4, true);
            Materia calculo = new Materia(276, "Calculo Financiero", 6, false);

            logica.MateriasSiguientes.Add(comunicaciones);
            logica.MateriasSiguientes.Add(tlya);
            dInfI.MateriasSiguientes.Add(dInfII);

            Carrera sistemas = new Carrera("Licenciatura en Sistemas de la informacion");
            sistemas.Plan.Add(logica);
            sistemas.Plan.Add(comunicaciones);
            sistemas.Plan.Add(tlya);
            sistemas.Plan.Add(dInfI);
            sistemas.Plan.Add(dInfII);
            sistemas.Plan.Add(calculo);

            Alumno juan = new Alumno("Juan", "Perez", 877340, sistemas);

            juan.cargarMateriaAprobada(logica);

            Curso comA = new Curso(655,12, comunicaciones, "Carlos Sanchez");
            Curso comB = new Curso(655,10, comunicaciones, "Sara Lopez");
            Curso TlA = new Curso(1652,7, tlya, "Jorge Gutierrez");
            Curso TlB = new Curso(1652,9, tlya, "Pablo Alvarez");
            Curso calA = new Curso(276,101, calculo, "Horacio Juarez");
            Curso calB = new Curso(276, 112, calculo, "Pedro Alfonso");
            Curso dereA = new Curso(1604,20, dInfII, "Mariana Gomez");

            Oferta of = new Oferta();
            of.OfertaAc.Add(comA);
            of.OfertaAc.Add(comB);
            of.OfertaAc.Add(TlA);
            of.OfertaAc.Add(TlB);
            of.OfertaAc.Add(calA);
            of.OfertaAc.Add(calB);
            of.OfertaAc.Add(dereA);

            //

            var arrayUsuarios = new Usuario[]
            {
                new Usuario(877340, "qw123"),
                new Usuario(123456, "test123"),
                new Usuario(654321, "asd123"),
                new Usuario(112233, "kevin321"),
                new Usuario(223344, "mateo789"),
                new Usuario(667788, "milagros2000"),
                new Usuario(889900, "qaz123"),

             };

          
            Console.WriteLine($"Bienvenidos al Sistema de Inscripción");
            Console.WriteLine();
            
            bool validaUsuario = false;
            
                do
                {                   

                    Console.WriteLine("Ingrese su número de Registro : ");
                    var ingresoRegistro = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(ingresoRegistro))
                    {
                        Console.WriteLine("Debe ingresar un número de Registro válido.");
                        return;
                    }

                    if (!ingresoRegistro.Any(c => Char.IsDigit(c)))
                    {
                        Console.WriteLine("Debe ingresar un número de registro válido.");
                        continue;
                    }

                    if (int.Parse(ingresoRegistro) < 0)
                    {
                        Console.WriteLine("Debe ingresar un número de registro válido.");
                        continue;
                    }

                    if (ingresoRegistro.Length != 6)
                    {
                        Console.WriteLine("Ingrese un número de registro que contenga 6 dígitos.");
                        continue;
                    }
                   

                    else

                    {

                            Console.WriteLine("Ingrese su contraseña:");
                            var ingresoContraseña = Console.ReadLine();


                            if (string.IsNullOrWhiteSpace(ingresoContraseña))
                            {
                                Console.WriteLine("Debe ingresar una contraseña válida.");
                                Console.WriteLine();
                                continue;
                            }

                    foreach (Usuario user in arrayUsuarios)
                    {
                        if (int.Parse(ingresoRegistro) == user.Registro && ingresoContraseña == user.contraseña)
                        {
                            validaUsuario = true;
                            int rtdo = MostrarMenu();
                             Boolean run = true;

                            while (run)
                            {
                                switch (rtdo)
                                {
                                    case 1:

                                        foreach (Curso c in of.OfertaAc)
                                        {
                                            Console.WriteLine("==================");
                                            Console.WriteLine("\nCodigo de Materia:" + c.CodigoMateria + ".\n");
                                            Console.WriteLine("Materia: " + c.Materia.Nombre + ".\n");
                                            Console.WriteLine("Profesor: " + c.Profesor + ".\n");
                                            Console.WriteLine("Codigo del curso: " + c.Code + ".\n");


                                        }
                                        rtdo = MostrarMenu();
                                        break;
                                    case 2:

                                        int cantInscriptas = 0;

                                        bool seguir = true;

                                        while (seguir)
                                        {

                                            Console.WriteLine("Ingrese el codigo de la materia a inscribirse:\n");
                                            validarInscripcion(Validar(Console.ReadLine()), of, juan, sistemas);
                                            cantInscriptas++;
                                            bool next = true;
                                            while (next && cantInscriptas < 4)
                                            {
                                                Console.WriteLine("\n¿Desea insribirse en otra materia?\n");
                                                Console.WriteLine("S - Si\n");
                                                Console.WriteLine("N -No\n");

                                                if (ValidarYN(Console.ReadLine()))
                                                {
                                                    Console.WriteLine("Ingrese el codigo de la materia a inscribirse:\n");
                                                    validarInscripcion(Validar(Console.ReadLine()), of, juan, sistemas);
                                                    cantInscriptas++;
                                                }
                                                else
                                                {
                                                    next = false;
                                                    rtdo = MostrarMenu();
                                                    seguir = false;
                                                    break;
                                                }
                                                break;
                                            }
                                            if (cantInscriptas == 4)
                                            {
                                                Console.WriteLine("Ya se incribió en 4 materias.\n");
                                                juan.HabilitadoInscripcion = false;
                                                break;
                                            }
                                        }

                                        rtdo = MostrarMenu();
                                        break;

                                    case 3:
                                        Console.WriteLine("Presione cualquier tecla para salir.\n");
                                        run = false;
                                        break;

                                    default:
                                        Console.WriteLine("Opción erronea. Intente de nuevo.\n");
                                        rtdo = MostrarMenu();
                                        break;
                                }
                                Console.ReadKey();
                            }

                            
                        }
                        

                    }
                    if (!validaUsuario)
                    {
                        Console.WriteLine("Ingreso al sistema incorrecto, verifique si ingreso correctamente su registro o contraseña.\n Por favor, intentelo nuevamente.");
                        Console.WriteLine();
                    }
                }


                } while (!validaUsuario);
            

            



        }

        public static int MostrarMenu()
        {
            Console.WriteLine("\n Menú: \n");
            Console.WriteLine("1- Mostrar oferta.\n");
            Console.WriteLine("2- Inscribirse.\n");
            Console.WriteLine("3- Salir.\n");
            String input = Console.ReadLine();
            return Validar(input);
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
                input = Console.ReadLine();
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

        public static void validarInscripcion(int code, Oferta of, Alumno a, Carrera carrera)
        {
            bool seguir = true;
            bool ok = false;
            int count = 0;
            code = ValidarMateria(code, carrera);
            code = validarRepetida(code, a);

            while (seguir)
            {
                foreach (Materia d in a.MateriasDisponibles)
                {
                    count++;
                    if (d.Code == code)
                    {

                        Console.WriteLine("Ingrese el codigo del curso original:\n");
                        Curso orig = ValidarCurso(Validar(Console.ReadLine()), of);

                        Console.WriteLine("¿Desea cargar un curso alternativo?\n");
                        Console.WriteLine("S - Si\n");
                        Console.WriteLine("N -No\n");

                        if (ValidarYN(Console.ReadLine()))
                        {

                            Console.WriteLine("Ingrese el codigo del curso alternativo:\n");
                            Curso alt = ValidarCurso(Validar(Console.ReadLine()), of);
                            Inscripcion insc = new Inscripcion(orig, alt);
                            a.MateriasInscriptas.Add(insc);
                            ok = true;
                            seguir = false;
                        }

                        else
                        {                 
                            Inscripcion insc = new Inscripcion(orig, null);
                            a.MateriasInscriptas.Add(insc);
                            ok = true;
                            seguir = false;
                        }
                        break;
                    }
                }
                if (count == a.MateriasDisponibles.Count && !ok)
                {

                    Console.WriteLine("Todavia no puede cursar esa materia. Intente con otra.\n");
                    code = Validar(Console.ReadLine());
                    validarInscripcion(code, of, a, carrera);

                }
            }
        }

  

        public static Curso ValidarCurso(int code, Oferta of)
        {
            int count = 0;
            bool seguir = true;
            Curso curso = null;
            while (seguir)
            {
                foreach (Curso c in of.OfertaAc)
                {
                    count++;
                    if (c.Code == code)
                    {
                        curso = c;
                        seguir = false;
                        Console.WriteLine("Curso cargado con exito.\n");
                        count = 0;
                        return curso;
                    }
                }
                if (count == of.OfertaAc.Count && curso == null)
                {
                    Console.WriteLine("Codigo erroneo. Intente de nuevo.\n");
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
            int aprob = 0;
            int insc = 0;
            while (!ok) {
                foreach (Materia m in a.MateriasAprobadas) {
                    aprob++;
                    if (m.Code == code) {

                        Console.WriteLine("Esa materia ya está aprobada. Intente con otra.\n");
                        code = Validar(Console.ReadLine());
                        break;
                }
                }
                foreach (Inscripcion i in a.MateriasInscriptas) {
                    insc++;

                    if (i.Alternativo != (null))
                    {

                        if (i.Original.Materia.Code == code || i.Alternativo.Materia.Code == code)
                        {

                            Console.WriteLine("Ya está inscripto a esa materia. Intente con otra.\n");
                            code = Validar(Console.ReadLine());
                            break;
                        }

                    }
                    else {

                        if (i.Original.Materia.Code == code)
                        {

                            Console.WriteLine("Ya está inscripto a esa materia. Intente con otra.\n");
                            code = Validar(Console.ReadLine());
                            break;
                        }
                    }
                   

                }
                if(aprob == a.MateriasAprobadas.Count && insc == a.MateriasInscriptas.Count)
                {
                    ok = true;
                }
            }
            return code;
        }
    }
        

        class Alumno {

        private String nombre;
        private String apellido;
        private Carrera carrera;
        private int registro;
        private List<Materia> materiasAprobadas = new List<Materia>();
        private List<Materia> materiasDisponibles = new List<Materia>();  //materias que tiene disponible para inscribirse
        private List<Inscripcion> materiasInscriptas = new List<Inscripcion>();
        private bool habilitadoInscripcion;

        public Alumno(string nombre, string apellido, int registro, Carrera carrera)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.registro = registro;
            this.carrera = carrera;

            foreach (Materia m in carrera.Plan) {
                if (!m.RequisitoPrevio) {
                    this.materiasDisponibles.Add(m);
                }
            }
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public int Registro { get => registro; set => registro = value; }
        public List<Materia> MateriasAprobadas { get => materiasAprobadas; set => materiasAprobadas = value; }
        public List<Materia> MateriasDisponibles { get => materiasDisponibles; set => materiasDisponibles = value; }
        public List<Inscripcion> MateriasInscriptas { get => materiasInscriptas; set => materiasInscriptas = value; }
        public bool HabilitadoInscripcion { get => habilitadoInscripcion; set => habilitadoInscripcion = value; }

        public void cargarMateriaAprobada(Materia m) {

            this.materiasAprobadas.Add(m);
            if (m.MateriasSiguientes.Count != 0)
            {
                foreach (Materia mProx in m.MateriasSiguientes) {
                    this.MateriasDisponibles.Add(mProx);
                }

            }

        }

    }

    class Materia {

        private String nombre;
        private int code;
        private List<Materia> materiasSiguientes = new List<Materia>();
        private int cargaHoraria;
        private bool requisitoPrevio;

        public Materia(int code, String nombre, int carga, bool requisitoPrevio)
        {
            this.code = code;
            this.nombre = nombre;
            this.cargaHoraria = carga;
            this.requisitoPrevio = requisitoPrevio;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public int Code { get => code; set => code = value; }
        public List<Materia> MateriasSiguientes { get => materiasSiguientes; set => materiasSiguientes = value; }
        public bool RequisitoPrevio { get => requisitoPrevio; set => requisitoPrevio = value; }
    }

    class Curso {
        private int codigoMateria;
        private Materia materia;
        private int code;
        private String profesor; //TODO: Crear clase de profesores
        //TODO: Agregar limitante de cupo -->   private int cupo;
        //TODO: Agregar horarios
        public Curso(int codigoMateria,int code, Materia materia, string profesor)
        {
            this.CodigoMateria = codigoMateria;
            this.materia = materia;
            this.Code = code;
            this.profesor = profesor;
        }

        public Materia Materia { get => materia; set => materia = value; }
        public string Profesor { get => profesor; set => profesor = value; }
        public int Code { get => code; set => code = value; }
        public int CodigoMateria { get => codigoMateria; set => codigoMateria = value; }
    }

    class Carrera {

        private String nombre;
        private List<Materia> plan = new List<Materia>();

        public Carrera(string nombre)
        {
            this.nombre = nombre;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public List<Materia> Plan { get => plan; set => plan = value; }
    }

    class Oferta {
    
    private List<Curso> ofertaAc = new List<Curso>();

        public List<Curso> OfertaAc { get => ofertaAc; set => ofertaAc = value; }
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
