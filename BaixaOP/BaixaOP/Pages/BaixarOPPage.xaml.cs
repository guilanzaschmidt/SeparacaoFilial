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
    public partial class BaixarOPPage : BasePage
    {
        private bool isCodigoBarrasOPCompleto = false;

        /*public BaixarOPPage(GerarCarga gerarcarga, UsuarioDTO usuario)
        {
            _gerarcarga = gerarcarga;
            _usuario = usuario;

            PreencherUsuario(_usuario.Usuario);

            InitializeComponent();
            BindingContext = this;

            FocarCampoInicial(txtCodigoBarrasOp);
        }*/
        public BaixarOPPage(MenuPage menupage, UsuarioDTO usuario)
        {
            _menuPage = menupage;
            _usuario = usuario;

            PreencherUsuario(_usuario.Usuario);

            InitializeComponent();
            BindingContext = this;            

            FocarCampoInicial(txtCodigoBarrasOp);
        }
        protected override bool OnBackButtonPressed() =>
            ClicarBotaoVoltar(txtCodigoBarrasOp.Text + txtQuantidade.Text);

        private void TxtCodigoBarrasOpOnEnterKeyPressed(object sender, EventArgs e)
        {
            isCodigoBarrasOPCompleto = false;

            if (ValidarCodigoBarrasOP())
            {

                txtQuantidade.Focus();
                //ExtrairNumeroOPCodigoBarrasOP(txtCodigoBarrasOp.Text.Trim());

                /*if (!string.IsNullOrEmpty(numeroOP))
                {
                    if (isCodigoBarrasOPCompleto)
                    {
                        VerificarSeOPMercado();
                    }
                    else
                    {
                        txtNumeroOperacao.Focus();
                        MostrarCamposNumeroOperacao();
                    }
                }
                else
                    LimparCampos();*/
            }
            else
                LimparCampos();
        }

        /*private void TxtNumeroOperacaoOnEnterKeyPressed(object sender, EventArgs e)
        {
            if (ValidarCodigoBarrasOP())
            {
                if (ValidarNumeroOperacao())
                    VerificarSeOPMercado();
                else
                {
                    txtNumeroOperacao.Text = string.Empty;
                    txtNumeroOperacao.Focus();
                }
            }
            else
                LimparCampos();
        }*/

        private void TxtQuantidadeOnEnterKeyPressed(object sender, EventArgs e)
        {
            if (ValidarQuantidade())
            {
                txtCodigoBarrasOp.Text += txtQuantidade.Text.Trim();                
                BaixarOP();
                /*if (ValidarCodigoBarrasOP())
                {
                    if (isCodigoBarrasOPCompleto)
                        EsconderCamposNumeroOperacao();

                    if ((string.IsNullOrEmpty(txtNumeroOperacao.Text) && isCodigoBarrasOPCompleto) || 
                    (!string.IsNullOrEmpty(txtNumeroOperacao.Text) && isCodigoBarrasOPCompleto))
                    {
                        if (grdNumeroOperacao.IsVisible)
                            ValidarNumeroOperacao();
                        
                        txtCodigoBarrasOp.Text += txtQuantidade.Text.Trim();

                        BaixarOP();
                    }
                    else if (!string.IsNullOrEmpty(txtCodigoBarrasOp.Text) && !isCodigoBarrasOPCompleto)
                    {
                        if(string.IsNullOrEmpty(txtNumeroOperacao.Text))
                            MostrarCamposNumeroOperacao();
                        
                        if (ValidarNumeroOperacao())
                            TxtQuantidadeOnEnterKeyPressed(sender, e);
                        else
                        {
                            txtNumeroOperacao.Text = string.Empty;
                            txtNumeroOperacao.Focus();
                        }
                    }
                }
                else
                {
                    txtCodigoBarrasOp.Text = string.Empty;
                    txtCodigoBarrasOp.Focus();
                }*/
            }
            else
            {
                txtQuantidade.Text = string.Empty;
                txtQuantidade.Focus();
            }
        }

        private void BtnFinalizarClicked(object sender, EventArgs e)
        {
            SeparacaoFinalizar();
        }

        private bool ValidarCodigoBarrasOP()
        {
            bool isValido = false;
            isCodigoBarrasOPCompleto = false;

            if (txtCodigoBarrasOp.Text.Trim().Length == 49)
                txtCodigoBarrasOp.Text = "0" + txtCodigoBarrasOp.Text;


            if (!string.IsNullOrEmpty(txtCodigoBarrasOp.Text))
            {
                if (txtCodigoBarrasOp.Text.Trim().Length == Constantes.QTDE_DIGITOS_CODIGO_DE_BARRAS_OP_COMPLETO &&
                txtCodigoBarrasOp.Text.Trim().All(char.IsDigit))
                {
                    isValido = true;
                    isCodigoBarrasOPCompleto = true;

                    txtCodigoBarrasOp.Text = txtCodigoBarrasOp.Text.Trim();
                }
                else if (txtCodigoBarrasOp.Text.Trim().Length == Constantes.QTDE_DIGITOS_CODIGO_DE_BARRAS_OP_UNICO /*&&
                txtCodigoBarrasOp.Text.Trim().All(char.IsDigit)*/)
                {
                    isValido = true;

                    txtCodigoBarrasOp.Text = txtCodigoBarrasOp.Text.Trim();
                }
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
                    quantidade = txtQuantidade.Text.TrimStart('0');

                    isValido = true;
                }
                else
                    DisplayAlert("Atenção!", "Quantidade não deve ser menor que 1.", "OK");
            }
            else
                DisplayAlert("Atenção!", "Informe a quantidade.", "OK");

            return isValido;
        }


        private async void BaixarOP()
        {
            if ((txtCodigoBarrasOp.Text.Trim().Length >= Constantes.QTDE_DIGITOS_CODIGO_DE_BARRAS_OP_COMPLETO) ||
                (txtCodigoBarrasOp.Text.Trim().Length >= Constantes.QTDE_DIGITOS_CODIGO_DE_BARRAS_OP_UNICO))
            {
                string Tipo;

                Tipo = "S";

                await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPage());

                var mensagemRetorno = SIDService.ExecutarRegraSeparacaoFilial(Tipo+txtCodigoBarrasOp.Text.Trim(), _usuario.Usuario, _usuario.Senha);
                
                await PopupNavigation.Instance.PopAsync();

                await DisplayAlert("Retorno", "OP: " + numeroOP + " - " + mensagemRetorno, "OK");
            }
            else
                await DisplayAlert("Atenção", "Não foi possível enviar a OP: " + numeroOP + ", faltam informações.", "OK");

            LimparCampos();
        }

        private async void SeparacaoFinalizar()
        {
            
                string Tipo = "F";

                await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPage());

                var mensagemRetorno = SIDService.ExecutarRegraSeparacaoFilial(Tipo + "00000000000", _usuario.Usuario, _usuario.Senha);

                await PopupNavigation.Instance.PopAsync();

                await DisplayAlert("Retorno", "OP: " + numeroOP + " - " + mensagemRetorno, "OK");
           
            
        }

        private void LimparCampos()
        {
            LimparCampos(txtCodigoBarrasOp, txtQuantidade);
        }

        private void ToggleCamposQuantidade(bool visivel)
        {
            grdQuantidade.IsVisible = visivel;
        }

    }
}