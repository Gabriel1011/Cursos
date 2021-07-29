using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
  class Program
  {
    static void Main(string[] args)
    {

      using var db = new Data.ApplicationContext();

      ConsultarPedidoCarregamentoAdiantado();
    }

    private static void RemoverRegistro()
    {
        using var db = new Data.ApplicationContext();

        //var cliente = db.Clientes.Find(2);
        var cliente = new Cliente { Id = 3};
        //db.Clientes.Remove(cliente);
        //db.Remove(cliente);
        db.Entry(cliente).State = EntityState.Deleted;

        db.SaveChanges();
    }

    private static void AtualizarDados()
    {
        using var db = new Data.ApplicationContext();
        //var cliente = db.Clientes.Find(1);

        var cliente = new Cliente
        {
            Id = 1
        };

        var clienteDesconectado = new
        {
            Nome = "Cliente Desconectado Passo 3",
            Telefone = "7966669999"
        };

        db.Attach(cliente);
        db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

        //db.Clientes.Update(cliente);
        db.SaveChanges();
    }

    private static void ConsultarPedidoCarregamentoAdiantado()
    {
        using var db = new Data.ApplicationContext();
        var pedidos = db
            .Pedidos
            .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
            .ToList();

        Console.WriteLine(pedidos.Count);
    }

    private static void CadastrarPedido()
    {
        using var db = new Data.ApplicationContext();

        var cliente = db.Clientes.FirstOrDefault();
        var produto = db.Produtos.FirstOrDefault();

        var pedido = new Pedido
        {
            ClienteId = cliente.Id,
            IniciadoEm = DateTime.Now,
            FinalizadoEm = DateTime.Now,
            Observacao = "Pedido Teste",
            Status = StatusPedido.Analise,
            TipoFrete = TipoFrete.SemFrete,
            Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
        };

        db.Pedidos.Add(pedido);

        db.SaveChanges();
    }

    private static void ConsultarDados(){
        using var db = new Data.ApplicationContext();
        //var ConsultarPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
        var consultaPorMetodo = db.Clientes
        .AsNoTracking()
        .Where(p => p.Id > 0)
        .OrderBy(p=>p.Id)
        .ToList();

        foreach (var cliente in consultaPorMetodo)
        {
            Console.WriteLine($"Consultando Cliente: {cliente.Id}");
            // db.Clientes.Find(Cliente.Id);
            db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
        }
    }

    private static void InserirDadosEmMassa()
    {
      var produto = new Produto
      {
        Descricao = "Produto Teste",
        CodigoBarras = "13413541454353",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
      };

      var cliente = new Cliente
      {
        Nome = "Gabriel Rodrogues",
        CEP = "2312312",
        Cidade = "Santos",
        Email = "gabriel.almeida@efcore.com",
        Estado = "SP",
        Telefone = "13999999999"
      };

      var clientes = new[] {
        new Cliente
        {
            Nome = "teste 1",
            CEP = "2312312",
            Cidade = "Santos",
            Email = "gabriel.almeida@efcore.com",
            Estado = "SP",
            Telefone = "13999999999"
        },
        new Cliente
        {
            Nome = "Teste 2",
            CEP = "2312312",
            Cidade = "Santos",
            Email = "gabriel.almeida@efcore.com",
            Estado = "SP",
            Telefone = "13999999999"
        },
      };

      using var db = new Data.ApplicationContext();
      //db.AddRange(produto, cliente);
      db.Clientes.AddRange(clientes);

      var registros = db.SaveChanges();
    }

    public static void InserirDados()
    {
      var produto = new Produto
      {
        Descricao = "Produto Teste",
        CodigoBarras = "13413541454353",
        Valor = 10m,
        TipoProduto = TipoProduto.MercadoriaParaRevenda,
        Ativo = true
      };

      using var db = new Data.ApplicationContext();
      db.Produtos.Add(produto);
      // db.Set<Produto>().Add(produto);
      // db.Set<Produto>().AddRange(produto);
      // db.Entry(produto).State = EntityState.Added;
      // db.Add(produto);

      var registros = db.SaveChanges();
    }
  }
}
