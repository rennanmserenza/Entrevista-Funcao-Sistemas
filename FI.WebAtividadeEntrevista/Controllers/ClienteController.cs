using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private bool VerificaBeneficiarioCpfDuplicado(ClienteModel model)
        {
            return model.Beneficiarios.GroupBy(b => b.CPF).Any(g => g.Count() > 1);
        }

        private void IncluirBeneficiarios(long idCliente, List<BeneficiarioModel> beneficiarioModels)
        {
            BoBeneficiario ben = new BoBeneficiario();

            beneficiarioModels
                .ForEach(x =>
                {
                    if (ben.CPFValido(x.CPF))
                    {
                        ben.Incluir(
                            new Beneficiario
                            {
                                CPF = x.CPF,
                                Nome = x.Nome,
                                IdCliente = idCliente
                            }
                        );
                    }
                });
        }

        private void AlterarBeneficiarios(long idCliente, List<BeneficiarioModel> beneficiarioModels)
        {
            BoBeneficiario ben = new BoBeneficiario();

            var beneficiariosExistentes = ben
                .ListarPorIdCliente(idCliente)
                .Select(b => new BeneficiarioModel
                {
                    Id = b.Id,
                    CPF = b.CPF,
                    Nome = b.Nome,
                    IdCliente = b.IdCliente,
                }).ToList();

            List<BeneficiarioModel> beneficiariosParaIncluir = new List<BeneficiarioModel>();
            List<BeneficiarioModel> beneficiariosParaAlterar = new List<BeneficiarioModel>();
            List<BeneficiarioModel> beneficiariosParaExcluir= new List<BeneficiarioModel>();

            beneficiarioModels
                .ForEach(x =>
                {
                    // Caso o beneficiario venha da tela com ID = 0,
                    // significa que o mesmo acabou de ser criado ou que ocorreu erro ao envia-lo para tela.
                    // caso ele exista no banco ele não será registrado.
                        if (x.Id != 0)
                            beneficiariosParaAlterar.Add(x);
                        else
                            if(beneficiariosExistentes.Count(y => y.CPF == x.CPF) == 0)
                                beneficiariosParaIncluir.Add(x);
                });

            beneficiariosParaIncluir
                .ForEach(x =>
                {
                    if (ben.CPFValido(x.CPF))
                    {
                        ben.Incluir(
                            new Beneficiario
                            {
                                CPF = x.CPF,
                                Nome = x.Nome,
                                IdCliente = idCliente
                            }
                        );
                    }
                });

            beneficiariosParaAlterar
                .ForEach(x =>
                {
                    ben.Alterar(
                        new Beneficiario
                        {
                            Id = x.Id,
                            Nome = x.Nome,
                            CPF = x.CPF,
                            IdCliente = idCliente
                        }
                    );
                });

            // Identificar beneficiários para excluir
            var cpfsParaIncluirAlterar = beneficiariosParaIncluir
                .Select(b => b.CPF)
                .Concat(beneficiariosParaAlterar.Select(b => b.CPF))
                .ToList();

            beneficiariosParaExcluir = beneficiariosExistentes
                .Where(existing => !cpfsParaIncluirAlterar.Contains(existing.CPF))
                .ToList();

            // Excluir beneficiários que não estão mais associados
            beneficiariosParaExcluir.ForEach(x =>
            {
                ben.Excluir(x.Id); // Supondo que você tenha um método Excluir na BoBeneficiario
            });
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if (VerificaBeneficiarioCpfDuplicado(model))
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "05EX01 - CPF duplicado entre beneficiários"));
            }
            else if (!bo.CPFValido(model.CPF))
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "05EX03 - O CPF inserido é inválido"));
            }
            else if (bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "05EX09 - Não foi possível incluir um novo cliente"));
            }
            else
            {
                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF,
                });

                if (model.Beneficiarios.Count > 0)
                {
                    IncluirBeneficiarios(model.Id, model.Beneficiarios);
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            Cliente cliente = boCliente.Consultar(id);
            ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = boBeneficiario.ListarPorIdCliente(cliente.Id).Select(b => new BeneficiarioModel
                    {
                        Id = b.Id,
                        CPF = b.CPF,
                        Nome = b.Nome,
                        IdCliente = b.IdCliente,
                    }).ToList()
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if (!bo.CPFValido(model.CPF))
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, "05EX07 - O CPF inserido é inválido"));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF,
                });

                if (model.Beneficiarios.Count > 0)
                {
                    AlterarBeneficiarios(model.Id, model.Beneficiarios);
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out int qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}