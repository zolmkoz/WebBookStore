<h1 align="center">Welcome to FPTBook  👋</h1>
<p>
<% if (isProjectOnNpm) { -%>
  <a href="https://www.npmjs.com/package/<%= projectName %>" target="_blank">
    <img alt="Version" src="https://img.shields.io/npm/v/<%= projectName %>.svg">
  </a>
E-commerce web application specializing in selling books of FPTBooks bookstore system
The system includes the following basic functions:
Log in sign up
Cart
CRUD product, cate
Book Details
Manage Users
Report admin
  
## 🤝 Contributing

Contributions, issues and feature requests are welcome!<br />Feel free to check [issues page](<%= issuesUrl %>). <%= contributingUrl ? `You can also take a look at the [contributing guide](${contributingUrl}).` : '' %>
<% } -%>
