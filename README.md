Desafio Técnico para Desenvolvedores – Intelectah
Objetivo: Desenvolver uma aplicação web para a gestão de concessionárias de veículos utilizando
Asp.net MVC e Entity Framework. O sistema deve permitir o gerenciamento de fabricantes de veículos,
veículos, concessionárias e a realização de vendas, integrando autenticação de usuários, relatórios e
otimização de desempenho.
Requisitos do Projeto
Funcionalidades Básicas (Essencial)
1. Autenticação e Autorização de Usuários
o Implementar login e registro de usuários com diferentes níveis de acesso (administrador,
vendedor, gerente).
o Utilizar Identity Framework para gerenciamento de usuários.
o Proteger rotas e ações específicas baseadas em permissões.
2. Cadastro de Fabricantes de Veículos (CRUD)
o Nome do Fabricante: Máximo de 100 caracteres, deve ser único.
o País de Origem: Campo de texto livre com no máximo 50 caracteres.
o Ano de Fundação: Deve ser um ano válido no passado.
o Website: Validação de URL.
3. Cadastro de Veículos (CRUD)
o Modelo: Nome do modelo, máximo de 100 caracteres.
o Ano de Fabricação: Validação para ano não futuro.
o Preço: Valor numérico positivo.
o Fabricante: Relação obrigatória com o fabricante.
o Tipo de Veículo: Enumeração (Carro, Moto, Caminhão etc.).
o Descrição: Texto opcional com no máximo 500 caracteres.
4. Cadastro de Concessionárias (CRUD)
o Nome da Concessionária: Máximo de 100 caracteres, deve ser único.
o Endereço Completo: Campos para rua, cidade, estado, e CEP com validação.
o Telefone: Validação de formato.
o E-mail: Validação de formato.
o Capacidade Máxima de Veículos: Valor inteiro positivo.
5. Integração com API Externa
o Integrar a aplicação com uma API de consulta de dados automotivos, como especificações
ou recall.
o Implementar chamadas assíncronas para a API utilizando AJAX.
o Um exemplo de uso seria API de CEP para o endereço.
6. Realização de Vendas
o Seleção de Concessionária: Pesquisa por nome ou localização.
o Seleção de Fabricante: Pesquisa por nome.
o Seleção de Veículo: Pesquisa por modelo. (O carregamento dos valores deste campo é
condicionado à seleção do valor do campo Fabricante. Utilize Ajax).
o Dados do Cliente: Nome, CPF (com validação e unicidade), e telefone.
o Data da Venda: Não pode ser futura.
o Preço de Venda: Deve ser menor ou igual ao preço do veículo.
o Geração de Número de Protocolo Único para a Venda.
Atenção: a deleção de registro dos CRUDs acima deve ser lógica.
Funcionalidades Avançadas (Opcional)
1. Relatórios e Dashboards
o Criar relatórios mensais de vendas realizadas, categorizados por tipo de veículo,
concessionária e fabricante.
o Dashboard inicial com gráficos sobre o desempenho de vendas, utilizando uma biblioteca
de gráficos (ex: Chart.js).
o Exportar relatórios para PDF ou Excel.
2. Otimização de Desempenho
o Implementar caching para consultas frequentes utilizando Redis ou Memcached.
o Melhorar o tempo de carregamento com lazy loading de imagens e recursos estáticos.
o Analisar e otimizar consultas SQL para desempenho.
3. Teste e Documentação
o Criar testes unitários e de integração para as principais funcionalidades.
o Documentar a API e endpoints com Swagger.
o Produzir documentação técnica do projeto explicando a arquitetura e decisões tomadas.
Tecnologias Utilizadas
 Frontend: Bootstrap, JavaScript (AJAX), HTML/CSS.
 Backend: ASP.NET MVC, Entity Framework.
 Banco de Dados: LocalDB ou SQL Server.
 Autenticação: Identity Framework.
 Caching: Redis ou Memcached.
 Relatórios: Chart.js ou similar.
 Documentação: Swagger para API, Markdown para documentação técnica.
Entrega
 O código deve ser publicado em um repositório GitHub público. (Essencial)
 Incluir instruções claras de como configurar e executar o projeto. (Opcional)
 Fornecer um vídeo ou demonstração do projeto em funcionamento. (Opcional)


Caso de Uso 1: Cadastro de Fabricante de Veículos
Ator Principal: Administrador
Objetivo: Permitir que o administrador adicione novos fabricantes de veículos ao sistema.
Fluxo Principal:
1. Início: O administrador faz login no sistema.
2. Ação: O administrador navega para a seção de "Fabricantes" e clica em "Adicionar Novo
Fabricante".
3. Entrada de Dados:
o Preenche o nome do fabricante.
o Seleciona o país de origem.
o Insere o ano de fundação.
o Adiciona o URL do website.
4. Validação: O sistema valida os campos, garantindo que:
o O nome do fabricante seja único e tenha no máximo 100 caracteres.
o O ano de fundação seja válido e no passado.
o O URL do website seja válido.
5. Confirmação: O administrador clica em "Salvar".
6. Resultado: O sistema adiciona o fabricante à base de dados e exibe uma mensagem de sucesso.
Fluxo Alternativo:
 Erro de Validação: Se algum campo for inválido, o sistema exibe mensagens de erro específicas e
impede o salvamento até que os erros sejam corrigidos.
Caso de Uso 2: Cadastro de Veículo
Ator Principal: Gerente de Concessionária
Objetivo: Permitir que o gerente adicione novos veículos ao estoque da concessionária.
Fluxo Principal:
1. Início: O gerente faz login no sistema.
2. Ação: O gerente navega para a seção de "Veículos" e clica em "Adicionar Novo Veículo".
3. Entrada de Dados:
o Insere o nome do modelo do veículo.
o Seleciona o ano de fabricação.
o Define o preço do veículo.
o Escolhe o fabricante associado.
o Seleciona o tipo de veículo.
o Adiciona uma descrição opcional.
4. Validação: O sistema verifica que:
o O nome do modelo tem no máximo 100 caracteres.
o O ano de fabricação não é no futuro.
o O preço é um valor numérico positivo.
o Um fabricante foi selecionado.
5. Confirmação: O gerente clica em "Salvar".
6. Resultado: O veículo é adicionado ao estoque e uma mensagem de confirmação é exibida.
Fluxo Alternativo:
 Erro de Validação: Caso algum dado esteja incorreto, o sistema impede o salvamento e exibe
mensagens de erro.
Caso de Uso 3: Cadastro de Concessionária
Ator Principal: Administrador
Objetivo: Registrar novas concessionárias no sistema.
Fluxo Principal:
1. Início: O administrador acessa o sistema.
2. Ação: O administrador vai para a seção de "Concessionárias" e clica em "Cadastrar Nova
Concessionária".
3. Entrada de Dados:
o Insere o nome da concessionária.
o Preenche o endereço completo (rua, cidade, estado, CEP).
o Adiciona o telefone e e-mail de contato.
o Define a capacidade máxima de veículos.
4. Validação: O sistema verifica:
o Nome é único e tem até 100 caracteres.
o CEP é válido.
o Telefone e e-mail estão em formato correto.
o Capacidade é um número positivo.
5. Confirmação: O administrador salva as informações.
6. Resultado: A nova concessionária é cadastrada e uma mensagem de sucesso é exibida.
Fluxo Alternativo:
 Erro de Validação: Se houver problemas nos dados, o sistema notifica o usuário para correção.
Caso de Uso 4: Realização de Venda
Ator Principal: Vendedor
Objetivo: Processar a venda de um veículo para um cliente.
Fluxo Principal:
1. Início: O vendedor faz login no sistema.
2. Ação: O vendedor acessa a seção de "Vendas" e clica em "Nova Venda".
3. Seleção de Veículo:
o Pesquisa o veículo por modelo ou fabricante.
o Seleciona o veículo desejado.
4. Seleção de Concessionária:
o Pesquisa a concessionária pelo nome ou localização.
o Seleciona a concessionária desejada.
5. Entrada de Dados do Cliente:
o Insere o nome do cliente.
o Insere o CPF (com validação de formato e unicidade).
o Insere o telefone de contato.
6. Definição da Venda:
o Seleciona a data da venda.
o Define o preço de venda (não superior ao preço do veículo).
7. Validação: O sistema valida todos os dados.
8. Confirmação: O vendedor clica em "Finalizar Venda".
9. Resultado:
o A venda é registrada.
o Um número de protocolo único é gerado.
o Uma mensagem de sucesso é exibida.
Fluxo Alternativo:
 Erro de Validação: Caso algum dado seja inválido, o sistema impede o prosseguimento e notifica
o vendedor para corrigir os dados.
Caso de Uso 5: Geração de Relatórios
Ator Principal: Gerente de Concessionária
Objetivo: Gerar relatórios de vendas mensais para análise.
Fluxo Principal:
1. Início: O gerente acessa o sistema e faz login.
2. Ação: Navega para a seção de "Relatórios" e seleciona "Relatório Mensal de Vendas".
3. Definição do Período:
o Escolhe o mês e ano para o relatório.
4. Geração de Relatório:
o O sistema processa os dados e gera um relatório com:
 Total de vendas.
 Vendas por tipo de veículo.
 Vendas por fabricante.
 Desempenho de cada concessionária.
5. Visualização e Exportação:
o O gerente visualiza o relatório na tela.
o Exporta o relatório para PDF ou Excel, se desejado.
6. Resultado:
o Relatório gerado e disponível para análise.
Fluxo Alternativo:
 Período Inválido: Se o período for inválido ou não houver dados, o sistema notifica o gerente e
não gera o relatório.
Caso de Uso 6: Autenticação de Usuários
Ator Principal: Todos os Usuários
Objetivo: Permitir que usuários acessem o sistema com segurança.
Fluxo Principal:
1. Início: O usuário acessa a página de login.
2. Entrada de Credenciais:
o Insere o nome de usuário e senha.
3. Autenticação:
o O sistema verifica as credenciais.
o Se corretas, o usuário é autenticado.
4. Acesso ao Sistema:
o O usuário é redirecionado para o dashboard conforme o nível de permissão.
o Ações e rotas são protegidas conforme o perfil (administrador, vendedor, gerente).
5. Resultado:
o Acesso ao sistema concedido com sucesso.
Fluxo Alternativo:
 Erro de Credenciais: Se as credenciais estiverem incorretas, o sistema exibe uma mensagem de
erro.


Modelagem de Dados
1. Tabela Fabricantes
 Descrição: Armazena informações sobre os fabricantes de veículos.
 Colunas:
o FabricanteID: INT (PK, Auto Increment) - Identificador único do fabricante.
o Nome: VARCHAR(100) - Nome do fabricante (deve ser único).
o PaísOrigem: VARCHAR(50) - País de origem do fabricante.
o AnoFundacao: INT - Ano de fundação do fabricante.
o Website: VARCHAR(255) - URL do site do fabricante.
2. Tabela Veiculos
 Descrição: Armazena detalhes dos veículos disponíveis.
 Colunas:
o VeiculoID: INT (PK, Auto Increment) - Identificador único do veículo.
o Modelo: VARCHAR(100) - Nome do modelo do veículo.
o AnoFabricacao: INT - Ano de fabricação do veículo.
o Preco: DECIMAL(10, 2) - Preço do veículo.
o FabricanteID: INT (FK) - Referência para o fabricante do veículo.
o TipoVeiculo: ENUM('Carro', 'Moto', 'Caminhão', etc.) - Tipo de veículo.
o Descricao: TEXT - Descrição opcional do veículo.
 Relacionamentos:
o FabricanteID é uma chave estrangeira que referencia FabricanteID em Fabricantes.
3. Tabela Concessionarias
 Descrição: Contém informações sobre as concessionárias.
 Colunas:
o ConcessionariaID: INT (PK, Auto Increment) - Identificador único da concessionária.
o Nome: VARCHAR(100) - Nome da concessionária (deve ser único).
o Endereco: VARCHAR(255) - Endereço completo da concessionária.
o Cidade: VARCHAR(50) - Cidade onde a concessionária está localizada.
o Estado: VARCHAR(50) - Estado onde a concessionária está localizada.
o CEP: VARCHAR(10) - CEP da concessionária.
o Telefone: VARCHAR(15) - Telefone de contato.
o Email: VARCHAR(100) - Email de contato.
o CapacidadeMaximaVeiculos: INT - Capacidade máxima de veículos que a concessionária
pode armazenar.
4. Tabela Clientes
 Descrição: Armazena informações sobre os clientes que realizam compras.
 Colunas:
o ClienteID: INT (PK, Auto Increment) - Identificador único do cliente.
o Nome: VARCHAR(100) - Nome completo do cliente.
o CPF: VARCHAR(11) - CPF do cliente (deve ser único e validado).
o Telefone: VARCHAR(15) - Telefone de contato do cliente.
5. Tabela Vendas
 Descrição: Armazena as informações das vendas realizadas.
 Colunas:
o VendaID: INT (PK, Auto Increment) - Identificador único da venda.
o VeiculoID: INT (FK) - Referência ao veículo vendido.
o ConcessionariaID: INT (FK) - Referência à concessionária onde a venda foi realizada.
o ClienteID: INT (FK) - Referência ao cliente que realizou a compra.
o DataVenda: DATETIME - Data e hora da venda.
o PrecoVenda: DECIMAL(10, 2) - Preço final de venda do veículo.
o ProtocoloVenda: VARCHAR(20) - Número de protocolo único para a venda.
 Relacionamentos:
o VeiculoID é uma chave estrangeira que referencia VeiculoID em Veiculos.
o ConcessionariaID é uma chave estrangeira que referencia ConcessionariaID em
Concessionarias.
o ClienteID é uma chave estrangeira que referencia ClienteID em Clientes.
6. Tabela Usuarios
 Descrição: Armazena informações sobre os usuários do sistema para autenticação e autorização.
 Colunas:
o UsuarioID: INT (PK, Auto Increment) - Identificador único do usuário.
o NomeUsuario: VARCHAR(50) - Nome de usuário.
o Senha: VARCHAR(255) - Senha criptografada.
o Email: VARCHAR(100) - Email do usuário.
o NivelAcesso: ENUM('Administrador', 'Vendedor', 'Gerente') - Nível de acesso do usuário.
Diagrama ER
Para visualizar o modelo de dados acima, você pode criar um diagrama de entidade-relacionamento (ER)
usando uma ferramenta como draw.io, Lucidchart, ou uma ferramenta ERD especializada. Aqui está uma
descrição textual do diagrama ER:
 Fabricantes
o (1,N) ---- (0,N) Veiculos
 Veiculos
o (1,1) ---- (0,N) Vendas
 Concessionarias
o (1,1) ---- (0,N) Vendas
 Clientes
o (1,1) ---- (0,N) Vendas
 Usuarios
o (Não diretamente relacionado a outras tabelas, mas utilizado para controle de acesso)
Considerações Adicionais
 Validação de Dados:
o CPF deve ser validado para garantir unicidade e formato correto.
o Preços e datas devem ser validados para evitar inconsistências.
 Desempenho e Indexação:
o Indexar colunas frequentemente pesquisadas, como CPF em Clientes, Nome em Veiculos,
e Nome em Concessionarias.
o Utilizar caching onde necessário para melhorar o desempenho em consultas frequentes.
 Segurança:
o Senhas devem ser criptografadas e armazenadas com segurança.
o Autenticação deve ser gerida por um framework como ASP.NET Identity.
Este modelo de dados abrange todas as funcionalidades requeridas no desafio técnico, garantindo uma
estrutura clara e lógica para o desenvolvimento da aplicação.