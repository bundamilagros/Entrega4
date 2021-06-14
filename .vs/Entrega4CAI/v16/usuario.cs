using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrega4CAI
{
    class Usuario
    {
        public int Registro { get; }
        public string contraseña { get; }

        public Usuario(int Registro, string contraseña)
        {
            this.Registro = Registro;
            this.contraseña = contraseña;
        }
    }
}
