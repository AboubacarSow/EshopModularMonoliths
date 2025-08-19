namespace Basket.Basket.Exceptions;
public class BasketNotFoundException(string Username):NotFoundException("ShoppingCart",Username){}