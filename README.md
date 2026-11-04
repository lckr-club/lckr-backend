# lckr-backend

O back-end é desenvolvido em C# com ASP.NET Core, utilizando Entity Framework Core para acesso e persistência dos dados.
A API foi desenvolvida para centralizar a gestão de coaches, aulas e pagamentos, garantindo que os relacionamentos entre essas informações sejam controlados pelo próprio sistema e reduzindo erros de registros manuais.

# API

Organizations: gerenciamento das unidades da organização, atualmente contemplando a LCKR.

Users: autenticação e controle de acesso dos usuários, como secretaria e administração.

Coaches: cadastro, consulta, atualização, inativação e reativação de coaches vinculados a uma organização.

Lessons: registro das aulas ministradas e do coach responsável, com bloqueio de horários sobrepostos e substituição de coach.

Coach Salary History: armazenamento do histórico de valores por hora, permitindo identificar o valor vigente em cada período.

Folha: cálculo mensal de horas e valores por coach, usando o valor vigente na data de cada aula realizada, com exportação em PDF e Excel.

Períodos: fechamento e reabertura do mês da folha. Ao fechar, as aulas do mês ficam bloqueadas e o sistema envia um e-mail para o e-mail da organização.

Payments: controle dos pagamentos dos coaches, gerados no fechamento do período, com data prevista no quinto dia útil do mês seguinte.

O back-end garante maior organização, consistência e rastreabilidade das informações de aulas e pagamentos.

# Como rodar com Docker

Com o Docker Desktop em execução, na pasta do projeto:

```
docker compose up --build
```

O banco é criado automaticamente com os scripts da pasta `database`.

- API: http://localhost:5180
- Swagger: http://localhost:5180/swagger
- Banco: `localhost:5434`, banco `lckr-database`, usuário `postgres`, senha `postgres`
- Login de teste: `secretaria@lckrclub.com.br`, senha `123456`

Para recriar o banco do zero:

```
docker compose down -v
docker compose up --build
```

# Como rodar sem Docker

1. Crie o banco `lckr-database` no PostgreSQL e execute os arquivos da pasta `database` na ordem 01, 02 e 03.
2. Crie o arquivo `API_LCKR/appsettings.Development.json`:

```
{
  "ConnectionStrings": {
    "Connection": "Host=localhost;Port=5432;Database=lckr-database;Username=postgres;Password=SUA_SENHA"
  }
}
```

3. Execute o perfil `http` pelo Visual Studio ou, na pasta `API_LCKR`:

```
dotnet run --launch-profile http
```

A API sobe em http://localhost:5180.

# Envio de e-mail

O e-mail de fechamento da folha é enviado para o e-mail da tabela `organization`. Para testar, altere esse valor no banco.

**Com Docker:** copie `.env.example` para `.env`, preencha e execute `docker compose up --build`.

```
EMAIL_HOST=smtp.gmail.com
EMAIL_PORTA=587
EMAIL_USUARIO=seuemail@gmail.com
EMAIL_SENHA=sua-senha-de-app
EMAIL_REMETENTE=seuemail@gmail.com
EMAIL_USAR_SSL=true
```

**Sem Docker:** adicione a seção `Email` ao `appsettings.Development.json`.

```
"Email": {
  "Host": "smtp.gmail.com",
  "Porta": 587,
  "Usuario": "seuemail@gmail.com",
  "Senha": "sua-senha-de-app",
  "Remetente": "seuemail@gmail.com",
  "UsarSsl": true
}
```

No Gmail, use uma senha de app (Conta Google, Segurança, Verificação em duas etapas, Senhas de app).
