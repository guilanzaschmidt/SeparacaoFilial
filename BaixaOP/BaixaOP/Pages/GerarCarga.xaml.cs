using SeparacaoFilial.DTO;
using SeparacaoFilial.Services;
using SeparacaoFilial.Utils;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace SeparacaoFilial.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GerarCarga : BasePage
    {
        public GerarCarga(UsuarioDTO usuario)
        {
            _usuario = usuario;

            LabelUsuario = _usuario.Usuario;

            InitializeComponent();
            BindingContext = this;
        }

        private async void BtnGerarCargaClicked(object sender, EventArgs e)
        {
            var resposta = await DisplayAlert("Atenção!", "Deseja realmente Gerar uma nova carga?", "Sim", "Não");

            if (resposta)
            {
                GerarCargaSep();
            }
        }
        private void BtnSepararClicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new BaixarOPPage(this, _usuario);
        }
        private void TxtCargaOnEnterKeyPressed(object sender, EventArgs e) => BtnSepararClicked(sender, e);

        private async void GerarCargaSep()
        {
            string Tipo = "S";

            await PopupNavigation.Instance.PushAsync(new ActivityIndicatorPage());

            var mensagemRetorno = SIDService.ExecutarRegraGerarCarga();

            await PopupNavigation.Instance.PopAsync();

            await DisplayAlert("Retorno", "OP: " + numeroOP + " - " + mensagemRetorno, "OK");
        }
    }
}