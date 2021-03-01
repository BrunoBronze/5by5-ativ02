using System;
using System.Globalization;
using System.IO;

namespace Covid_19
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declara Arquivo , cria diretório ou não.
            ArquivoCSV arquivo = new ArquivoCSV();
            arquivo.Path = @"C:\temp\ws-c#\5by5-ativ02\Pacientes.csv";

            if (!Directory.Exists(@"C:\temp\ws-c#\5by5-ativ02"))
            {
                // OBS: E se não tiver permissão para criar o diretório????
                Directory.CreateDirectory(@"C:\temp\ws-c#\5by5-ativ02");
            }
            if (!File.Exists(arquivo.Path))
            {
                FileStream file = File.Create(arquivo.Path);
                file.Close();
            }

            Paciente paciente = new Paciente();
            FilaPacientes fila = new FilaPacientes();
            FilaPacientes filaPrioritaria = new FilaPacientes();

            FilaPacientes urgente = new FilaPacientes();
            FilaPacientes poucoUrgente = new FilaPacientes();
            FilaPacientes naoUrgente = new FilaPacientes();

            FilaPacientes assintomaticos = new FilaPacientes();
            // ´Várias filas e um único arquivo?

            int contador = 0;

            string op;
            do
            {
                Console.WriteLine(" >>>BEM VINDOS AO HOSPITAL DE CAMPANHA COVID 19<<<\n" +
                                  "\n1 - Cadastre um paciente\n" +
                                  "2 - Proximo da fila\n" +
                                  "3 - Chamar para internacao\n" +
                                  "4 - Encerrar programa");
                Console.Write("\n>>>");
                op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        //Fluxo Triagem Inicial
                        Console.Clear();
                        Console.Write("Informe o CPF: ");
                        string cpf = Console.ReadLine();

                        // Só faz a leitura do arquivo para verificar se o paciente passou por ali
                        // Não tem como saber quem são as pessoas internadas
                        // Ficou bem "vago" essa situação sem imprimir dados da doença dos pacientes
                        // Foi pedido para gravar o restante para manter um Histórico

                        if (arquivo.ProcuraCPF(cpf) != -1)
                        {
                            paciente = arquivo.Leitura(cpf);
                            Imprimir(paciente);
                        }
                        else
                        {
                            paciente = Leitura(cpf);
                        }

                        //Fluxo da separação da fila
                        if (paciente.Idade() >= 60)  // ok - Fizeram controle da idade!!
                        {                            // Usuário é brasileiro, além de tudo!!!
                            filaPrioritaria.Push(paciente);
                        }
                        else
                        {
                            fila.Push(paciente);
                        }
                        break;

                    case "2":
                        Console.Clear();

                        //Fluxo de fila
                        //ok - Fizeram controle de proporção de chamada em filas - ok

                        if (!filaPrioritaria.Vazia() && contador < 2)
                        {
                            Console.WriteLine("Chamando próximo paciente para exame...");
                            paciente = filaPrioritaria.Head;
                            filaPrioritaria.Pop();    // ok - Mostra Nome de Paciente. e a senha(?) 
                            contador++;               // na hora de chamar

                            Imprimir(paciente);
                            Infectado(paciente, urgente, poucoUrgente, naoUrgente, assintomaticos, arquivo);
                        }
                        else if (!fila.Vazia())
                        {
                            Console.WriteLine("\nChamando próximo paciente para exame...");
                            paciente = fila.Head;
                            fila.Pop();
                            contador = 0;

                            Imprimir(paciente);
                            Infectado(paciente, urgente, poucoUrgente, naoUrgente, assintomaticos, arquivo);
                        }
                        else
                        {
                            Console.WriteLine("\n>>> Não há ninguem na fila! <<<");
                            contador = 0;
                        }

                        break;

                    case "3":
                        Console.Clear();
                        // ok - Separou a internacao em 3 filas
                        // Dúvida: Se o programa parar de executar, como temos acesso às internações?

                        if (!urgente.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para internação...\n");
                            Console.WriteLine($"Chamando o paciente {urgente.Head.Nome} para internação");
                            urgente.Pop();
                        }

                        else if (!poucoUrgente.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para internação...\n");
                            Console.WriteLine($"Chamando o paciente {poucoUrgente.Head.Nome} para internação");
                            poucoUrgente.Pop();
                        }

                        else if (!naoUrgente.Vazia())
                        {
                            Console.WriteLine("Chamando próximo paciente para internação...\n");
                            Console.WriteLine($"Chamando o paciente {naoUrgente.Head.Nome} para internação");
                            naoUrgente.Pop();
                        }

                        else
                        {
                            Console.WriteLine("\n>>> Sem ninguém na fila para internação!! <<<");
                        }

                        break;

                    case "4":
                        Console.WriteLine(">>> FINALIZANDO <<<");
                        break;

                    default:
                        Console.WriteLine("\nDigite uma opção contida no menu !\n");
                        break;
                }
            } while (op != "4");
        }

        static Paciente Leitura(string cpf)
        {
            Console.WriteLine("CPF não cadastrado, insira os dados: ");

            Console.Write("Nome :");
            string nome = Console.ReadLine();

            DateTime dataNascimento = DateTime.Parse("01/01/0001");
            do //ok 
            {
                Console.Write("Data de nascimento(dd/mm/aaaa): ");
                string dn = Console.ReadLine();
                
                if (!DateTime.TryParseExact(dn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascimento))
                {
                    Console.WriteLine("Digite no formato especificado: dd/mm/aaaa");
                }
            } while (dataNascimento == DateTime.Parse("01/01/0001"));


            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Paciente paciente = new Paciente
            {
                Nome = nome,
                CPF = cpf,
                DataNascimento = dataNascimento,
                Telefone = telefone,
                Proximo = null
            };

            Console.WriteLine();
            return paciente;
        }

        static void Imprimir(Paciente paciente)
        {
            Console.Clear();
            Console.WriteLine(paciente); // Sem os dados da avaliação/triagem???
            Console.WriteLine();
        }


        // Questionamento: Esses métodos não ficariam melhores distribuidos em classes??
        static void Infectado(Paciente paciente, FilaPacientes urgente, FilaPacientes poucoUrgente, FilaPacientes naoUrgente, FilaPacientes assintomaticos, ArquivoCSV arquivo)
        {
            paciente.VerificaStatus();
            // Só grava os dados de paciente infectado e após a internação
            // E se o sistema tiver problema até essa etapa?????
            // Muito confuso, pois não separa os pacientes em arquivos distintos para termos o histórico
            // Além disso, os pacientes acabam voltando para poder internar novamente sem ter alta? E a alta?
            // E se o sistema der problema durante o dia, com ou sem triagem? 
            // Como recuperar as informações - era um dos quesitos
            if (paciente.Covid)
            {
                Console.Write("\nPaciente está com sintomas?[S/N]: ");
                string sintomas = Console.ReadLine().ToUpper();

                if (sintomas == "S")
                {
                    paciente.Importancia();
                    Console.Write("\nAnalisando Urgência do paciente e adicionando em fila para internação...\n");

                    if (paciente.Comorbidade)
                    {
                        if (paciente.Periodo > 12)
                        {
                            urgente.Push(paciente);
                        }
                        else
                        {
                            poucoUrgente.Push(paciente);
                        }
                    }
                    else if (paciente.Periodo > 12)
                    {
                        poucoUrgente.Push(paciente);
                    }
                    else
                    {
                        naoUrgente.Push(paciente);
                    }  // Que Lógica complexa da Urgência! Doidinha!

                    int posicao = arquivo.ProcuraCPF(paciente.CPF);
                    if (posicao != -1)
                    {
                        arquivo.Salvar(paciente, posicao);
                    }
                    else
                    {
                        arquivo.Salvar(paciente);
                    }
                }
                else
                {
                    assintomaticos.Push(paciente);

                    // Dúvida: Aonde difere o assintomático do paciente
                    // com sintomas????
                    // Só vai saber se estiver internado?
                    // E se a pessoa voltar???
                    Console.WriteLine("\nArquivando paciente...\n");
                    int posicao = arquivo.ProcuraCPF(paciente.CPF);
                    if (posicao != -1)
                    {
                        arquivo.Salvar(paciente, posicao);
                    }
                    else
                    {
                        arquivo.Salvar(paciente);
                    }

                }
            }
            else
            {
                Console.WriteLine("\nArquivando paciente...\n");
                if (arquivo.ProcuraCPF(paciente.CPF) == -1)
                {
                    arquivo.Salvar(paciente);
                }
            }
        }
    }
}