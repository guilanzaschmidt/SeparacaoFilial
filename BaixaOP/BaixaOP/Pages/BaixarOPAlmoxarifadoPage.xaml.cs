using SeparacaoFilial.DTO;
using SeparacaoFilial.Services;
using SeparacaoFilial.Utils;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace SeparacaoFilial.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaixarOPAlmoxarifadoPage : BasePage
    {
        public BaixarOPAlmoxarifadoPage(MenuPage menuPage, UsuarioDTO usuario)
        {
            _usuario = usuario;
            _menuPage = menuPage;


            LabelUsuario = _usuario.Usuario;

            InitializeComponent();
            BindingContext = this;

            FocarCampoInicial(txtCodigoBarrasOp);
        }

        protected override bool OnBackButtonPressed() =>
            ClicarBotaoVoltar(txtCodigoBarrasOp.Text + txtQuantidade.Text);

        private void TxtCodigoBarrasOpOnEnterKeyPressed(object sender, EventArgs e)
        {
            if (ValidarCodigoBarrasOP())
            {
                txtQuantidade.Focus();
            }
            else
                LimparCampos(txtCodigoBarrasOp, txtQuantidade);
        }

        private void TxtQuantidadeOnEnterKeyPressed(object sender, EventArgs e)
        {
            if (ValidarCodigoBarrasOP())
            {               
                txtQuantidade.Focus();
                if (ValidarQuantidade())
                {
                    string nValor = "00";
                    txtQuantidade.Text = nValor += txtQuantidade.Text;
                    txtCodigoBarrasOp.Text += txtQuantidade.Text.Trim();
                    txtCodigoBarrasOp.Text = txtCodigoBarrasOp.Text.Replace("\n", "");
                    BaixarOP();
                }
                else
                {
                    txtQuantidade.Text = string.Empty;
                    txtQuantidade.Focus();
                }
            }
            else
                LimparCampos(txtCodigoBarrasOp, txtQuantidade);
        }
        private void BtnFinalizarClicked(object sender, EventArgs e)
        {
            SeparacaoFinalizar();
        }

        private bool ValidarCodigoBarrasOP()
        {
            bool isValido = false;

            if (!string.IsNullOrEmpty(txtCodigoBarrasOp.Text))
            {
                if (txtCodigoBarrasOp.Text.Trim().All(char.IsDigit))
                    isValido = true;
                else
                    DisplayAlert("Erro!", "Código de barras inválido.", "OK");
            }
            else
                DisplayAlert("Atenção!", "Informe o número do código de barras.", "OK");

            return isValido;
        }

        private bool ValidarQuantidade()
        {
            bool isValido = false;

            if (!string.IsNullOrEmpty(txtQuantidade.Text))
            {
                if (int.Parse(txtQuantidade.Text.Trim()) > 0)
                {
                    isValido = true;
                }
                else
                    DisplayAlert("Atenção!", "Quantidade não deve ser menor que 1.", "OK");
            }
            else
                DisplayAlert("Atenção!", "Informe a quantidade.", "OK");

            return isValido;
        }

        private void ExtrairQuantidadeCodigoBarrasOP()
        {
            try
            {
                quantidade = txtCodigoBarrasOp.Text.Trim().Substring(11, 4);
            }
            catch
            {
                quantidade = string.Empty;
                DisplayAlert("Erro!", "Falha ao extrair a quantidade do código de barras.", "OK");
            }
        }

        private async void BaixarOP()
        {
            if (txtCodigoBarrasOp.Text.Trim().Length >= Constantes.QTDE_DIGITOS_CODIGO_DE_BARRAS_ACESSORIOS)
            {
                string Tipo;

                Tipo = "V";

                await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPage());

                var mensagemRetorno = SIDService.ExecutarRegraSeparacaoFilial(Tipo + _usuario.Filial + txtCodigoBarrasOp.Text.Trim(), _usuario.Usuario, _usuario.Senha);

                await PopupNavigation.Instance.PopAsync();

                await DisplayAlert("Retorno", "OP: " + numeroOP + " - " + mensagemRetorno, "OK");
            }
            else
                await DisplayAlert("Atenção", "Não foi possível enviar a OP: " + numeroOP + ", faltam informações.", "OK");

            LimparCampos(txtCodigoBarrasOp, txtQuantidade);
        }

        private async void SeparacaoFinalizar()
        {

            string Tipo = "F";

            await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPage());

            var mensagemRetorno = SIDService.ExecutarRegraSeparacaoFilial(Tipo + "00000000000", _usuario.Usuario, _usuario.Senha);

            await PopupNavigation.Instance.PopAsync();

            await DisplayAlert("Retorno", "OP: " + numeroOP + " - " + mensagemRetorno, "OK");


        }
    }
}