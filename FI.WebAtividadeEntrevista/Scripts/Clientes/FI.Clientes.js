let beneficiarios = [];

$(document).ready(function () {
    // Evento de clique para adicionar beneficiário
    $('#formDeBeneficiario').submit(function (e) {
        e.preventDefault();

        let cpf = $('#CPFBeneficiario').val();
        let nome = $('#NomeBeneficiario').val();

        // Verifica se o CPF já está listado
        if (beneficiarios.some(b => b.CPF === cpf)) {
            ModalDialog("Erro", "Este CPF já foi adicionado para o beneficiário.");
            return;
        }

        // Adiciona beneficiário à lista e atualiza a tabela
        beneficiarios.push({ CPF: cpf, Nome: nome });
        atualizarTabelaBeneficiarios();

        // Limpa os campos da modal
        $('#CPFBeneficiario').val('');
        $('#NomeBeneficiario').val('');
    });

    // Atualizar tabela de beneficiários
    function atualizarTabelaBeneficiarios() {
        $('#tabelaDeBeneficarios').empty();
        beneficiarios.forEach((b, index) => {
            $('#tabelaDeBeneficarios').append(`
                <tr>
                    <td>${b.CPF}</td>
                    <td>${b.Nome}</td>
                    <td><button type="button" class="btn btn-warning btn-sm" onclick="alterarBeneficiario(${index})">Alterar</button>
                    <button type="button" class="btn btn-danger btn-sm" onclick="removerBeneficiario(${index})">Remover</button></td>
                </tr>
            `);
        });
    }

    // Função para alterar beneficiário
    window.alterarBeneficiario = function (index) {
        ModalDialog("Aviso", "Esta função ainda não foi desenvolvida para o projeto.");
    }

    // Função para remover beneficiário
    window.removerBeneficiario = function (index) {
        if (confirm("Tem certeza que deseja excluir este beneficiário?")) {
            beneficiarios.splice(index, 1);
            atualizarTabelaBeneficiarios();
        }
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "CPF": $(this).find("#CPF").val(),
                "CEP": $(this).find("#CEP").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Email": $(this).find("#Email").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "Beneficiarios": beneficiarios // Envio dos beneficiarios vinculados ao cliente.
            },
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
            function (r) {
                ModalDialog("Sucesso!", r)
                $("#formCadastro")[0].reset();
                beneficiarios = []; // Limpa a lista de beneficiários após o envio
                atualizarTabelaBeneficiarios();
            }
        });
    })

    // Função para aplicar a máscara de CPF
    function aplicarMascaraCPF(value) {
        // Remove tudo que não é dígito
        value = value.replace(/\D/g, "");

        // Adiciona os pontos e o traço no lugar correto
        value = value.replace(/(\d{3})(\d)/, "$1.$2");
        value = value.replace(/(\d{3})(\d)/, "$1.$2");
        value = value.replace(/(\d{3})(\d{2})$/, "$1-$2");

        return value;
    }

    $('#CPF').on("input change", function() {
        $(this).val(aplicarMascaraCPF($(this).val()));
    });
})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
