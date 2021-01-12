using ApiProdutos.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:44395/api/produtos/");
            response.EnsureSuccessStatusCode();
            var produtos = await response.Content.ReadFromJsonAsync<IEnumerable<Produto>>();
            produtosListView.ItemsSource = produtos;
        }

        private async void btnNovoProduto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var produto = new Produto()
                {
                    Id = int.Parse(txtIDProduto.Text),
                    Nome = txtNomeProduto.Text,
                    Preco = decimal.Parse(txtPreco.Text),
                    Estoque = int.Parse(txtEstoque.Text),
                };

                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync("https://localhost:44395/api/produtos/", produto);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Produto cadastrado com sucesso.", "Result", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                produtosListView.ItemsSource = await GetAllProdutos();
                produtosListView.ScrollIntoView(
                    produtosListView.ItemContainerGenerator.Items[produtosListView.Items.Count - 1]);

                txtIDProduto.Text = String.Empty;
                txtNomeProduto.Text = String.Empty;
                txtPreco.Text = String.Empty;
                txtEstoque.Text = String.Empty;
            }
            catch (Exception)
            {
                MessageBox.Show("O produto não foi cadastrado. ID informado já existe.");
            }
        }

        private async void btnAtualiza_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var produto = new Produto()
                {
                    Id = int.Parse(txtIDProduto.Text),
                    Nome = txtNomeProduto.Text,
                    Preco = decimal.Parse(txtPreco.Text),
                    Estoque = int.Parse(txtEstoque.Text),
                };

                using var client = new HttpClient();
                var response = await client.PutAsJsonAsync("https://localhost:44395/api/Produtos/" + txtID.Text, produto);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Produto atualizado com sucesso.", "Result", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                produtosListView.ItemsSource = await GetAllProdutos();
                produtosListView.ScrollIntoView(
                    produtosListView.ItemContainerGenerator.Items[produtosListView.Items.Count - 1]);

                txtIDProduto.Text = String.Empty;
                txtNomeProduto.Text = String.Empty;
                txtPreco.Text = String.Empty;
                txtEstoque.Text = String.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnGetProduto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("https://localhost:44395/api/produtos/" + txtID.Text);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                Produto resultProduto = JsonConvert.DeserializeObject<Produto>(result);
                txtIDProduto.Text = resultProduto.Id.ToString();
                txtNomeProduto.Text = resultProduto.Nome;
                txtPreco.Text = resultProduto.Preco.ToString();
                txtEstoque.Text = resultProduto.Estoque.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Produto não localizado.");
            }
        }

        private async void btnDeletaProduto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.DeleteAsync("https://localhost:44395/api/Produtos/" + txtID.Text);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Produto excluído com sucesso.", "Result", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                produtosListView.ItemsSource = await GetAllProdutos();
                produtosListView.ScrollIntoView(
                    produtosListView.ItemContainerGenerator.Items[produtosListView.Items.Count - 1]);

                txtIDProduto.Text = String.Empty;
                txtNomeProduto.Text = String.Empty;
                txtPreco.Text = String.Empty;
                txtEstoque.Text = String.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task<IEnumerable<Produto>> GetAllProdutos()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:44395/api/produtos/");
            response.EnsureSuccessStatusCode();
            var produtos = await response.Content.ReadFromJsonAsync<IEnumerable<Produto>>();
            return produtos;
        }
    }
}
