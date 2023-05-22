# InventoryManagement
 InventoryManagement.Api
 ![image](https://user-images.githubusercontent.com/70817318/234557828-4e47c707-2d26-4e80-bea8-a374eec18cb3.png)
 InventoryManagement.Auth
 ![image](https://user-images.githubusercontent.com/70817318/234558232-fb3df314-c667-46f5-ac76-4a84ebc144c0.png)

<br>
<ul>
  <li>
    InventoryManagement is a solution for a shop, who wish to manage their inventory.
  </li>
  <li>
    It contains two APIs-
    <ul>
      <li>InventoryManagement.Api</li>
      <li>InventoryManagement.Auth
      </li>
    </ul>
  </li>
  <li>
    A RESTful Web API, created with ASP.NET Core, EF core, MySQL.
  </li>
  <li>The API code is production level.</li>
  <li>The API code follows 3 layered architecture.</li>
  <li>Middlewares for Token validation and Exception Handling.</li>
  <li>The API has two roles- Consumer and Admin.</li>
 <li>Consumer can only see his own orders, whereas an admin can see every consumers' orders</li>
 <li>An admin can add a product, update a product, delete a product whereas consumer cannot.</li>
</ul>
