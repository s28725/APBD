using System.Data;
using APBD.Model;
using APBD.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APBD.Controllers;

[ApiController]
[Route("api/warehouses/")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private IConfiguration _configuration;

    public WarehouseController(IWarehouseRepository warehouseRepository, IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _warehouseRepository = warehouseRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    [HttpPost]
    public IActionResult AddProductToWarehouse(ProductDto productDto)
    {
        Product product = _productRepository.ProductExists(productDto.IdProduct);
        if (product == null 
            || !_warehouseRepository.WarehouseExists(productDto.IdWarehouse))
        {
            return NotFound();
        }

        Order order = _orderRepository.HasValidPurchaseOrder(productDto.IdProduct, productDto.Amount, productDto.CreatedAt);
        if (order == null) 
        {
            return NotFound();
        }

        if (_orderRepository.IsOrderCompleted(order.IdOrder))
        {
            return StatusCode(201);
        }
        
        _orderRepository.UpdateOrderFulfilledDate(order.IdOrder);

        var generatedId = _warehouseRepository.InsertIntoProductWarehouse(
            productDto.IdProduct, 
            productDto.IdWarehouse, 
            productDto.Amount, 
            product.Price);
        
        return StatusCode(201, generatedId); 
    }
    
    [HttpPost]
    public IActionResult AddProductToWarehouseSecondTask(ProductDto productDto)
    {
        Product product = _productRepository.ProductExists(productDto.IdProduct);
        if (product == null || !_warehouseRepository.WarehouseExists(productDto.IdWarehouse))
        {
            return NotFound();
        }

        Order order = _orderRepository.HasValidPurchaseOrder(productDto.IdProduct, productDto.Amount, productDto.CreatedAt);
        if (order == null) 
        {
            return NotFound();
        }

        if (_orderRepository.IsOrderCompleted(order.IdOrder))
        {
            return StatusCode(201);
        }

        // Call the stored procedure
        int generatedId = _warehouseRepository.ExecuteAddProductToWarehouseStoredProcedure(productDto.IdProduct, productDto.IdWarehouse, productDto.Amount, product.Price, productDto.CreatedAt);

        return StatusCode(201, generatedId); 
    }
}