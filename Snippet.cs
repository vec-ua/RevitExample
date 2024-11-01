//Coding task: Evaluate the next code snippet for violation of Design Principles. 
//Highlight places in the code for improvement.
 
public class OrderService
{
    public static IServiceLocator ServiceLocator { get; set; }
    private string _accountGuid;
    public OrderService(string accountGuid)
    {
        _accountGuid = accountGuid;
    }
 
    public async void CreateOrderAsync(Guid usedId, string userEmail, Guid accountId, int price, int amount, string ModelIP, string[] data)
    {
        try
        {
            if (accountId == Guid.Parse(_accountGuid))
            {
                if (price == 0 || price < 0)
                    throw new Exception("Invalid price exception");
                if (amount == 0 || amount > 10)
                    throw new Exception("Invalid amount exception");
 
                System.IO.StreamWriter file = new System.IO.StreamWriter(ModelIP);
                foreach (string dataV in data)
                {
                    // If the line doesn't contain the word 'Time', write the line to the file
                    if (!dataV.Contains("Time"))
                    {
                        file.WriteLine(dataV);
                    }
                }
 
                var o = new Order();
                var i2 = "@gmail.com";
                o.Price = price * amount + 2.15;
                o.Path = ModelIP;
                var ro = ServiceLocator.GetInstance<IRepositoryOrder>();
                if (userEmail.Contains(i2))
                {
                    o.Price = o.Price * 0.0124;
                }
 
                var r = await ro.SavingExecute(o, ModelIP, userEmail, usedId, accountId);
 
 
            }
            else
            {
                throw new Exception("This account does not support order creation.");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
