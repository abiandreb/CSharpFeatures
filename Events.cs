
// The easiest way to declare an event is to put the event keyword in front of a delegate member.
// Code within the containing type has full access and can treat the event as a delegate.
// Code outside of the containing type can only perform += and -= operations on the event.

var stock = new Stock ("MSFT");
stock.PriceChanged += ReportPriceChange;
stock.Price = 123;
stock.Price = 456;

void ReportPriceChange (decimal oldPrice, decimal newPrice)
{
	("Price changed from " + oldPrice + " to " + newPrice).Dump();
}

public delegate void PriceChangedHandler (decimal oldPrice, decimal newPrice); //Should not return any value

public class Stock
{
	string symbol;
	decimal price;
	
	public Stock (string symbol) { this.symbol = symbol; }
	
	public event PriceChangedHandler PriceChanged;
	
	public decimal Price
	{
		get { return price; }
		set
		{
			if (price == value) return;			// Exit if nothing has changed
			decimal oldPrice = price;
			price = value;
			if (PriceChanged != null)			// If invocation list not empty,
				PriceChanged (oldPrice, price);	// fire event.
		}
	}
}

/************************************************************************/
//Standart event Pattern 

// There's a standard pattern for writing events. The pattern provides
// consistency across both Framework and user code.

Stock stock = new Stock ("THPW");
stock.Price = 27.10M;
// Register with the PriceChanged event
stock.PriceChanged += stock_PriceChanged;
stock.Price = 31.59M;
stock.Price = 40.50M;

void stock_PriceChanged (object sender, PriceChangedEventArgs e)
{
	if ((e.NewPrice - e.LastPrice) / e.LastPrice > 0.1M)
		Console.WriteLine ("Alert, 10% stock price increase!");
}

public class PriceChangedEventArgs : EventArgs
{
	public readonly decimal LastPrice;
	public readonly decimal NewPrice;
	
	public PriceChangedEventArgs (decimal lastPrice, decimal newPrice)
	{
		LastPrice = lastPrice; NewPrice = newPrice;
	}
}

public class Stock
{
	string symbol;
	decimal price;
	
	public Stock (string symbol) {this.symbol = symbol;}
	
	public event EventHandler<PriceChangedEventArgs> PriceChanged; //Default delegate/event EventHandler. Should not return any value
	
	protected virtual void OnPriceChanged (PriceChangedEventArgs e)
	{
		PriceChanged?.Invoke (this, e);
	}
	
	public decimal Price
	{
		get { return price; }
		set
		{
			if (price == value) return;
			decimal oldPrice = price;
			price = value;
			OnPriceChanged (new PriceChangedEventArgs (oldPrice, price));
		}
	}
}
