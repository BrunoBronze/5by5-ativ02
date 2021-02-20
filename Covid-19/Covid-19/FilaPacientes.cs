using System;


namespace Covid_19
{
    class FilaPacientes
    {
        public Paciente Head { get; set; }
        public Paciente Tail { get; set; }

        public bool Vazia()
        {
            if ((Head == null) && (Tail == null))
                return true;
            return false;
        }

        virtual public void Push(Paciente aux)
        {
            if (Vazia())
            {
                Head = aux;
                Tail = aux;
            }
            else
            {
                Tail.Proximo = aux;
                Tail = aux;
            }
            Console.WriteLine("Elemento Inserido com sucesso!!!");
        }

        public void Pop()
        {
            if (Vazia())
            {
                Console.WriteLine("\nImpossível Remover! Fila Vazia!");
            }
            else
            {
                Head = Head.Proximo;
                Console.WriteLine("\nElemento Removido com Sucesso!");
                if (Head == null)
                {
                    Tail = null;
                    Console.WriteLine("Fila agora está vazia!");
                }
            }
        }
        public void Print()
        {
            if (Vazia())
            {
                Console.WriteLine("\nImpossível Imprimir! Fila Vazia!");
            }
            else
            {
                Paciente aux = Head;
                Console.WriteLine("\n>>>AS Ordens de Serviço são<<<\n");
                do
                {
                    Console.WriteLine(aux.ToString());
                    aux = aux.Proximo;
                } while (aux != null);
                Console.WriteLine("\n>>> FIM DA IMPRESSÃO <<<");
            }
        }
    }
}