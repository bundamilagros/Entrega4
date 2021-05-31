﻿using System;
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

            Alumno juan = new Alumno("Juan", "Perez", 414243, sistemas);

            juan.cargarMateriaAprobada(logica);

            Curso comA = new Curso(12, comunicaciones, "Carlos Sanchez");
            Curso comB = new Curso(10, comunicaciones, "Sara Lopez");
            Curso TlA = new Curso(7, tlya, "Jorge Gutierrez");
            Curso TlB = new Curso(9, tlya, "Pablo Alvarez");
            Curso cal = new Curso(101, calculo, "Horacio Juarez");
            Curso dereA = new Curso(20, dInfII, "Mariana Gomez");

            Oferta of = new Oferta();
            of.OfertaAc.Add(comA);
            of.OfertaAc.Add(comB);
            of.OfertaAc.Add(TlA);
            of.OfertaAc.Add(TlB);
            of.OfertaAc.Add(cal);
            of.OfertaAc.Add(dereA);

            //

            int rtdo = MostrarMenu();
            Boolean run = true;

            while (run)
            {
                switch (rtdo)
                {
                    case 1:

                        foreach (Curso c in of.OfertaAc) {
                            Console.WriteLine("\nMateria: " + c.Materia.Nombre +".\n");
                            Console.WriteLine("Profesor: " + c.Profesor + ".\n");
                            Console.WriteLine("Codigo del curso: " + c.Code + ".\n");
                        }
                        rtdo = MostrarMenu();
                        break;
                    case 2:

                        int cantInscriptas=0;

                        bool seguir = true;

                        while (seguir) {

                            Console.WriteLine("Ingrese el codigo de la materia a inscribirse:\n");
                            validarInscripcion(Validar(Console.ReadLine()), of, juan);
                            cantInscriptas++;
                            bool next = true;
                            while (next&& cantInscriptas<4) {
                                Console.WriteLine("\n¿Desea insribirse en otra materia?\n");
                                if (ValidarYN(Console.ReadLine()))
                                {
                                    Console.WriteLine("Ingrese el codigo de la materia a inscribirse:\n");
                                    validarInscripcion(Validar(Console.ReadLine()), of, juan);
                                    cantInscriptas++;
                                }
                                else {
                                    next = false;
                                    rtdo = MostrarMenu();
                                }
                            }
                            if (cantInscriptas == 4) {
                                Console.WriteLine("Ya se incribió en 4 materias.\n");
                                juan.HabilitadoInscripcion = false;
                            }
                        }

                        rtdo = MostrarMenu();
                        break;
                    case 3:
                        Console.WriteLine("Presione cualquier tecla para salir.\n");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Opción erronea. Intente de nuevo.\n");
                        rtdo = MostrarMenu();
                        break;
                }
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

        public static int MostrarMenu()
        {
            Console.WriteLine("\n Menú: \n");
            Console.WriteLine("1- Mostrar oferta.\n");
            Console.WriteLine("2- Inscribirse.\n");
            Console.WriteLine("3- Salir.\n");
            String input = Console.ReadLine();
            return Validar(input);
        }

        public static void validarInscripcion(int code, Oferta of, Alumno a)
        {

            bool ok = false; 
            bool disp = false;
            int contador = 0;

            while (!ok || !disp) { 

            foreach (Curso c in of.OfertaAc)
            {
                if (c.Code == code)
                {
                        ok = true;
                        int count = 0;
                       

                    foreach (Materia d in a.MateriasDisponibles)
                    {
                     count++;

                        if (d.Equals(c.Materia))
                        {
                            a.MateriasInscriptas.Add(c.Materia);
                            Console.WriteLine("Inscripcion exitosa.\n");
                            disp = true;
                            break;
                        }    
                    }

                        if (count == a.MateriasDisponibles.Count && !disp) {
                            Console.WriteLine("Todavia no puedes cursar esa materia. Intente de nuevo.\n");
                            code = Validar(Console.ReadLine());
                        }     
                }
            }
            if (contador == of.OfertaAc.Count && !ok)
            {
                Console.WriteLine("Codigo erroneo. Intente de nuevo.\n");
                code = Validar(Console.ReadLine());
            }
        }
        }
    }

    class Alumno {

        private String nombre;
        private String apellido;
        private Carrera carrera;
        private int registro;
        private List<Materia> materiasAprobadas = new List<Materia>();
        private List<Materia> materiasDisponibles = new List<Materia>();  //materias que tiene disponible para inscribirse
        private List<Materia> materiasInscriptas = new List<Materia>();
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
        public List<Materia> MateriasInscriptas { get => materiasInscriptas; set => materiasInscriptas = value; }
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

        private Materia materia;
        private int code;
        private String profesor; //TODO: Crear clase de profesores
        //TODO: Agregar limitante de cupo -->   private int cupo;
        //TODO: Agregar horarios
        public Curso(int code, Materia materia, string profesor)
        {
            this.materia = materia;
            this.Code = code;
            this.profesor = profesor;
        }

        public Materia Materia { get => materia; set => materia = value; }
        public string Profesor { get => profesor; set => profesor = value; }
        public int Code { get => code; set => code = value; }
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
}
