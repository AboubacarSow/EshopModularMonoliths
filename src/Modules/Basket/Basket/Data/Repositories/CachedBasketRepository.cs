using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Data.Repositories;

internal class CachedBasketRepository(IBasketRepository basketRepository,IDistributedCache cache) 
: IBasketRepository
{
    private static JsonSerializerOptions _options=> new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new ShoppingCartItemConverter(), new ShoppingCartConverter() }
        };
    
    public async Task<Guid> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.CreateBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket,_options), cancellationToken);
        return result;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await basketRepository.DeleteBasket(userName,cancellationToken);
        await cache.RemoveAsync(userName,cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (trackChanges)
            return await basketRepository.GetBasket(userName, true, cancellationToken);

        //check if the data is in the cache
        await cache.RemoveAsync(userName,cancellationToken) ;    
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
        }

        //if not
        var basket = await basketRepository.GetBasket(userName, trackChanges, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket,_options), cancellationToken);
        return basket;
    }

    public async Task<List<string>> GetBasketUserNamesByItemId(Guid ProductId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await basketRepository.GetBasketUserNamesByItemId(ProductId, trackChanges, cancellationToken);
    }

    public async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default,params string?[] userNames)
    {
        var result=await basketRepository.SaveChangesAsync(cancellationToken,userNames);
        if (userNames is not null)
        {
            foreach (var userName in userNames)
            {
                await cache.RemoveAsync(userName!, cancellationToken);
            }
                       
        }
        return result;
    }

    public async Task<bool> UpdateItemPriceInBasket(Guid productId, decimal price, CancellationToken cancellationToken = default)
    {
        return await basketRepository.UpdateItemPriceInBasket(productId, price, cancellationToken);
    }
}