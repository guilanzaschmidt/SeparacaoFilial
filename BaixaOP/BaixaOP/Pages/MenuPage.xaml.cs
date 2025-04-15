using SeparacaoFilial.DTO;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparacaoFilial.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : BasePage
    {
        public MenuPage(UsuarioDTO usuario)
        {
            _usuario = usuario;

            LabelUsuario = _usuario.Usuario;

            InitializeComponent();
            BindingContext = this;
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new LoginPage();
            return true;
        }

        private void BtnBaixarOPClicked(object sender, EventArgs e)
        {
          Application.Current.MainPage = new BaixarOPPage(this, _usuario);
        }

        private void pckFilial_SelectedIndexChanged(object sender, EventArgs e) => BtnBaixarOPAlmoxarifadoClicked(sender, e);

        private void BtnBaixarOPAlmoxarifadoClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new BaixarOPAlmoxarifadoPage(this, _usuario);                
        }
    }
}