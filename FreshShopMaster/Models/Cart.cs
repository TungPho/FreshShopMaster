namespace FreshShopMaster.Models
{	
	public class Cart
	{
		public List<CartLine> lines = new List<CartLine>();
		
		//methods:
		//1. Add item, delete item, compute total price, and clear all the items
		public void AddItem(Product product, int quantity)
		{
			//find the Cartline base on product id acording to the product passed in:
			CartLine? line = lines.Where(id => id.Product.ProductId == product.ProductId).FirstOrDefault();
			//if not found, create a new one
			if (line == null)
			{	
				lines.Add(new CartLine()
				{
					Product = product,
					Quantity = quantity
				});
			}
			// if found, increase the quantity:
			else
			{
				line.Quantity += 1;
			}
		}
		//2. Delete a line: 
		public void RemoveLine(Product product)
		{
			lines.RemoveAll(cartline => cartline.Product.ProductId == product.ProductId);
		}
		//3.Tinh tong tien mat hang:
		public decimal ComputeTotalPrice()
		{
			var total = lines.Sum(cartLine => cartLine.Product.ProductPrice * cartLine.Quantity);
		return total;
		}

		public decimal TotalDiscount()
		{
			var total = lines.Sum(cartLine => (cartLine.Product.ProductPrice * cartLine.Product.ProductDiscount) * cartLine.Quantity);
			return total;
		}

		//4 Delete all lines
		public void ClearAll()
		{
			lines.Clear();
		}

        public void ReduceItem(Product product)
        {
            //find the Cartline base on product id acording to the product passed in:
            
			CartLine? line = lines.Where(id => id.Product.ProductId == product.ProductId).FirstOrDefault();
			if(line.Quantity > 0)
			{
                line.Quantity -= 1;
            }
			
        }

    }
	//A CartLine is a compound of data(obj) (Produtc, quantity and id)
	public class CartLine
	{
        public int CartLineId { get; set; }
        
		public Product Product { get; set; } = new Product();
		public int Quantity { get; set; }
    }
}
