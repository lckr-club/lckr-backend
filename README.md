# lckr-backend

O back-end é desenvolvido em C# com ASP.NET Core, utilizando Entity Framework Core para acesso e persistência dos dados.
A API foi desenvolvida para centralizar a gestão de coaches, aulas e pagamentos, garantindo que os relacionamentos entre essas informações sejam controlados pelo próprio sistema e reduzindo erros de registros manuais.

# API

Organizations: gerenciamento das unidades da organização, atualmente contemplando a LCKR.

Users: autenticação e controle de acesso dos usuários, como secretaria e administração.

Coaches: cadastro, consulta, atualização e exclusão de coaches vinculados a uma organização.

Lessons: registro das aulas ministradas e do coach responsável.

Coach Salary History: armazenamento do histórico de valores por aula, permitindo identificar o valor vigente em cada período.

Payments: controle dos pagamentos dos coaches, utilizando o valor vigente na data de cada aula.

O back-end garante maior organização, consistência e rastreabilidade das informações de aulas e pagamentos.
