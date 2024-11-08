# OpenAI API Middleware

Este projeto tem como objetivo criar uma API intermediária utilizando .NET, que se conecta à OpenAI. A ideia principal é aprender e praticar o desenvolvimento com .NET enquanto se constrói uma solução que permite reutilizar a integração com a API da OpenAI em diversos projetos, sem a necessidade de expor a chave da API diretamente em cada um deles.

## Funcionalidades

- **Autenticação Segura:** Armazena a chave da API da OpenAI em variáveis de ambiente para garantir que informações sensíveis não sejam expostas no código-fonte.
- **Endpoints para Modelos:** Permite listar os modelos disponíveis na OpenAI e gerar respostas com base em prompts fornecidos.
- **Geração de Imagens:** Integração com o endpoint de geração de imagens da OpenAI (`/v1/images/generations`), onde as imagens geradas são salvas em um banco de dados MongoDB.
- **Validações Robustas:** Implementa validações para garantir que os dados enviados para a API estejam corretos e adequados.

## Tecnologias Utilizadas

- .NET 8.0
- ASP.NET Core
- C#
- OpenAI API
- MongoDB

## Como Usar

1. **Clone o Repositório:**
   ```bash
   git clone https://github.com/thadeucbr/MyOpenAIIntegrationAPI.git
   cd MyOpenAIIntegrationAPI
   ```

2. **Criar um Arquivo `.env`:**
   Na raiz do projeto, crie um arquivo chamado `.env` e adicione suas variáveis de configuração:
   ```plaintext
   OPENAI_API_KEY=your_openai_api_key_here
   OPENAI_BASE_URL=https://api.openai.com/v1
   MONGODB_CONNECTION_STRING=mongodb://192.168.1.239:27017
   MONGODB_DATABASE_NAME=MyOpenAiIntegrationAPI
   ```

3. **Executar o Projeto:**
    - Abra o terminal e execute o projeto:
   ```bash
   dotnet run
   ```

4. **Verificar as Rotas:**
    - Após executar o projeto, você pode visualizar e testar as rotas disponíveis usando a interface do Swagger. Acesse: `http://localhost:5000/swagger` no seu navegador.

## Contribuições

Sinta-se à vontade para contribuir com melhorias, correções de bugs ou novas funcionalidades! Para isso:

1. Faça um fork do repositório.
2. Crie uma nova branch (`git checkout -b minha-feature`).
3. Faça suas alterações e commit (`git commit -m 'Adicionando uma nova feature'`).
4. Envie para o repositório remoto (`git push origin minha-feature`).
5. Abra um Pull Request.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).