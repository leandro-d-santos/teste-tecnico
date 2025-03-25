# Controle de acesso de API de integração

## Contexto 
A empresa REV, especializada na revenda de materiais, identificou a necessidade de otimizar a integração de seus sistemas internos com plataformas externas. Para isso, contratou um desenvolvedor para criar uma API de integração que permita a consulta e o cadastro de clientes e pedidos, além de um painel administrativo para controle de acesso à API.

## Requisitos

### API de integração
- [ ] O acesso à API deve ser controlado por um token gerado no painel administrativo.
- [ ] A utilização do token de acesso deve seguir o padrão `Bearer Token`.
- [ ] A API deve permitir consulta e cadastro de clientes e pedidos.
- [ ] O formato das respostas deve seguir o padrão JSON.
- [ ] A API deve permitir atualizar apenas o status do pedido.
- [ ] A API deve permitir filtrar os clientes fornecendo filtros.
- [ ] A API deve permitir filtrar os pedidos fornecendo filtros.
- [ ] A API deve permitir paginação dos clientes.
- [ ] A API deve permitir paginação dos pedidos.

### Painel Administrativo
- [ ] O painel deve possuir login utilizando `BASIC AUTH`.
- [ ] O painel deve permitir geração de tokens de acesso.
- [ ] O painel deve permitir revogação de tokens de acesso.

### Contratos
#### Clientes
``` json
// Contrato de cadastro/atualização de clientes
// REQUEST
{
  "nome": "string",
  "email": "string",
  "telefone": "string",
  "endereco": {
    "rua": "string",
    "numero": "string",
    "cidade": "string",
    "estado": "string",
    "cep": "string"
  }
}
// RESPONSE
{
  "id": 0,
  "nome": "string",
  "status": "Ativo"
}
```
``` json
// Contrato de consulta de clientes
[
  {
    "id": 0,
    "nome": "string",
    "email": "string",
    "telefone": "string",
    "status": "Ativo"
  }
]
```

#### Pedidos
``` json
// Contratos de cadastro/atualização de pedidos
// REQUEST
{
  "cliente_id": 0,
  "itens": [
    {
      "produto_id": 0,
      "quantidade": 0,
      "preco_unitario": 0.0
    }
  ]
}
// RESPONSE
{
  "pedido_id": 0,
  "cliente_id": 0,
  "status": "Em Processamento"
}
```
``` json
// Contrato de consulta de pedidos
[
  {
    "pedido_id": 0,
    "cliente_id": 0,
    "itens": [
      {
        "produto_id": 0,
        "quantidade": 0,
        "preco_unitario": 0.0
      }
    ],
    "valor_total": 0.0,
    "status": "Em Processamento"
  }
]
```
#### TOKENS
``` json
// Contrato de cadastro de tokens
// REQUEST
{
  "descricao": "string",
  "expiracao": "2025-12-31T23:59:59Z"
}
// RESPONSE
{
  "token": "string"
}
```
``` json
// Contrato de consulta de tokens
[
  {
    "descricao": "string",
    "expiracao": "2025-12-31T23:59:59Z"
  }
]
```

### Entregas
Deve ser entregue o link do github via e-mail (`leandro.santos@movtech.com.br`).
<br>
**No repositório deve conter:**
- Projeto frontend/backend
- Scripts de migração inicial do banco de dados

### Regras
- Não utilizar ORM ou MicroORM
- Pode ser utilizado qualquer banco relacional de sua preferência
- O backend deve ser escrito em .Net
- O frontend deve ser escrito em Angular

## Diferenciais (opcional) 
- Dockerizar a aplicação
- Teste unitário utilizando XUnit
- Registrar log para auditoria
- Restringir acesso do token por IP