using Cashbox.Core;
using Cashbox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly Product _product;
        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public static async Task<List<ProductViewModel>> GetProducts() => await Product.GetProducts();
        public static async Task<List<ProductViewModel>> GetAllProducts() => await Product.GetAllProducts();
        public static async Task<ProductViewModel?> CreateProduct(string? _ArticulCode, string _Title, string _Description, byte[]? _Image, string _Brand, int _CategoryId, double _PurchaseСost, double _SellCost, int _Amount) => await Product.CreateProducts(_ArticulCode, _Title, _Description, _Image, _Brand, _CategoryId, _PurchaseСost, _SellCost, _Amount);
        public static async Task<ProductViewModel?> UpdateProduct(int id, string? ArticulCode, string Title, string Description, byte[]? Image, string Brand, int CategoryId, double PurchaseСost, double SellCost, int Amount) => await Product.UpdateProducts(id, ArticulCode, Title, Description, Image, Brand, CategoryId, PurchaseСost, SellCost, Amount);
        public static async Task<ProductViewModel?> RemoveProduct(int id) => await Product.AvailableProducts(id, false);
        public static async Task<ProductViewModel?> UnRemoveProduct(int id) => await Product.AvailableProducts(id, true);

        public int Id => _product.Id;

        public string? ArticulCode 
        {
            get => _product.ArticulCode;
            set
            {
                _product.ArticulCode = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _product.Title;
            set
            {
                _product.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _product.Description;
            set
            {
                _product.Description = value;
                OnPropertyChanged();
            }
        }

        public byte[]? Image
        {
            get => _product.Image;
            set
            {
                _product.Image = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage ImageGet
        {
            get
            {
                BitmapImage image = new();
                byte[] data = Image;
                if (data == null)
                    data = Convert.FromBase64String("89504E470D0A1A0A0000000D4948445200000100000001000802000000D3103F31000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000147649444154785EED9DE94F54D71BC7CB08B2AA80C52A29EE28D658ABE88B5A95B669E29236266DAD6DD22636AD49FF1D5F344D9336B6458BA0E2129722C8303828A0D6AD5A59874D64DF46661861F0F74DEFF919A332F7CE7A97F3FDBCB0732EC30C94E7F39CE739E7DE3B714F9F3E7D8D1059B189FF1222251480480D052052430188D4500022351480480D052052430188D4500022351480480D052052430188D4500022351480480D052052430188D4500022351480480D052052430188D4500022351480480D052052430188D4500022351480480D052052430188D4500022351480480D052052430188D444EC0332FC7EFFC4C484C7E3191818181D1DC5E3E9E969F13542C223292969CE9C39AFBFFE7A7A7A3A1EC7C5C5892F844DB802E0DB8787877B7A7AEEDDBBD7DEDE8EC7535353087D1C0FF395095140B8039BCD161F1F9F9A9ABA78F1E2BCFFC0E3F04D085D007CA3DBEDAEAFAFBF73E74E7F7F3FE23EFC9F86105510789061DEBC791B376EDCB469D3FCF9F3C309BC1005F0F97C77EFDEADA9A9E9EEEE66A6277A919191B17DFBF6FCFC7CD445E250908422C0D0D050656525123F0A7D7188109D40FA7FE79D7776ECD8919999290E0543D002F4F6F69E3D7BB6A5A505358F384488DEE4E4E47CF6D967D9D9D962AC99E00440F49794943C7CF8902B3CC450208CB3B2B2BEFEFAEB601D08621F607474F4DCB9738C7E62405008F5F7F7233B0F0C0C8843DAD02A80D7EB75381C8D8D8D8C7E624CE0407777F7F9F3E7C7C7C7C5210D6812C0EFF7373434DCBC7993D14F0CCEFDFBF7AF5DBB86881563353409303636565B5BFBF8F1633126C4A82047D7D7D7F7F5F589B11AEA02A0BDB87BF76E7B7B3BF7B98829181C1CBC71E386C66A455D008FC773FBF66D2E7A12B38094FDEFBFFF0E0D0D8971405404C06BB5B5B5F5F4F430FD13138149A0A9A949CB12BF8A0098479A9B9B9F3C7922C6849801C4ED83070FB4B4C22A1B61687C7FFDF5D7AEAEAEA066003C39212161D6AC59624C4818208E272727B5A4F3E7C9C8C8F8E1871FF0AF18CF808A008F1E3DFAF9E79FD10688B106D2D2D2F2F3F357AC58317BF66C7188903040FF893ABCB6B6D6ED766B4FC448C1070E1C58BA74A918CF808A000D0D0DBFFFFEBBF60E1841BF77EFDEBCBCBCC4C444718890B0514AF1A2A222ED9B5C5065DFBE7D1B366C08EC8C4A0F80F7C3EC23066AC0A58D1B37AE5BB78ED14F228BCD66CBCDCD2D282810630D201AC7C6C6540B271501D0FE6A5C4F05500D950F7E563126247220BA56AD5A151F1F2FC61AF07ABDE2D1CCA804ABF62D6580D067EE27D10365BDF6F48ADCAF257A99AD89D4500022351480480D052052430188D4500022351480480D052052430188D450002235E613E0E9D3A71313132323237D7D7D6D6D6D8D8D8D0D0D0DCDCDCD0F1F3E1C1E1EF67ABDAAE73F11F20C95D3A16B6A6A4A4B4B355EDA62B3D9F6EFDF9F979727C611657A7AFAF1E3C7BDBDBD88F5EEEE6E3C40ACFBFD7EE5E78F8B8BC30F191F1F9F9E9E9E9D9DBD64C99265CB966564640475EE143138FDFDFD070F1ED4787D2202A3A0A060F7EEDD814F1F3281009393935D5D5D77EEDC41B2478E574ECF0E7092377E23FC24292929D060E3C68DB9B9B991FD4805A217D110C0D02510B27E6B6B2B0C3C7CF830547CF6290481A3195FC52F3F3E3E7EFFFEFD23478EFCF8E38FB76EDDE28DACC92B31AE0088E00B172E1C3D7AF4C68D1B6EB75B1C0D12988056012F023A3B3B034F7744428C2800C2141D6D6161617575359A5D71340CF082F7EEDD3B74E8D0CD9B3783BAC281581EC309A0042B4A17143F114CD8A88BD04317151555545468BFC893581E63098088FFE79F7F4E9C3811EC4DAE35020DCACBCBCF9E3D4B078882810440CBDBD0D070EAD4A9A06E6F1D2C369B0DFDF4993367580B1160140190FB3B3A3ACE9D3B1772BFAB1D3880C6BAB6B6368225163129461100CDAEDD6ED77E57EB30999A9A422DE472B9C498C88A210440458E94ACF16EA691C2E3F19C3C7992FB03926308013A3B3BFFFEFBEFD8DF81BDB7B7F7CA952B2C8464467F017C3E1FA27F7070508C638BD3E9D47823796249F417E0D1A3470D0D0DDAEF3F1759D073A3FAE224202D3A0B80B2E7DEBD7BA3A3A3814FEF891E78DFDBB76FC760E98918139D05181B1B6B6C6CD43701A3046A6D6D150322193A0BD0D3D3333030A057FA57C02CD4D2D2C27D3139D1530024FE8E8E0EDD3F7F09FAB5B7B76BB99330B11E7A0A303939890E580C740595D8F0F0B0185811BDD6188C8F9E02F87CBEBEBE3E7DEB1F05A47FBDD6616380C7E3713A9DA836C5983C879E0220EC0CB2118B04393A3A2A06D6C2ED765754545CB870A1B4B4B4BFBF5F1C25FF474F0110FDB1DFFD9D89F1F171EBED0620C55455555DBB760D2D7E5B5BDBC993277B7B7BC5D7C87FE82900A2DF3831875EDC6202A0B141EEAFABAB43A9A91C696E6EC63C10A56B2D4C8A9E02182AE02C16FDA87C1C0E07A2FF85453697CB55545414910B4DAD819E02CC9A35CB081DB08295EE2084E8B7DBEDB5B5B5AF5C62EEECEC2C2C2CB470D31F147A0A909494649C4F934F4949318E8DE180BA1FD18FDC1FE0B2CF8E8E8EA3478FF22C40A0A700C9C9C9C6F934F9B973E75A4000047D595919727FE08B9EF19BA2272E2E2EE6BA909E0220FAD3...");
                using (var mem = new MemoryStream(data))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
        }

        public string Brand
        {
            get => _product.Brand;
            set
            {
                _product.Brand = value;
                OnPropertyChanged();
            }
        }

        public int CategoryId
        {
            get => _product.CategoryId;
            set
            {
                _product.CategoryId = value;
                OnPropertyChanged();
            }
        }

        public double PurchaseСost
        {
            get => _product.PurchaseСost;
            set
            {
                _product.PurchaseСost = value;
                OnPropertyChanged();
            }
        }

        public double SellCost
        {
            get => _product.SellCost;
            set
            {
                _product.SellCost = value;
                OnPropertyChanged();
            }
        }
        public bool IsAvailable
        {
            get => _product.IsAvailable;
            set
            {
                _product.IsAvailable = value;
                OnPropertyChanged();
            }
        }

        public virtual ProductCategory? Category
        {
            get => _product.Category;
            set
            {
                _product.Category = value;
                OnPropertyChanged();
            }
        }

        public virtual Stock? Stock
        {
            get => _product.Stock;
            set
            {
                _product.Stock = value;
                OnPropertyChanged();
            }
        }

    }
}
