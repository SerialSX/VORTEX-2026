# NavegadorVortex360

Protótipo funcional de navegador panorâmico interativo em 360°, desenvolvido em Unity para o desafio técnico do processo seletivo de estágio em Jogos do Laboratório Vortex (UNIFOR). Replica o funcionamento do Google Street View, permitindo navegação entre 10 imagens panorâmicas reais de um trecho da Av. Domingos Olímpio, Fortaleza-CE.

**Candidato:** João Arthur de Abreu Souza
**Engine:** Unity 6.5 (6000.5.7f1), template Universal 3D (URP)

---

## Requisitos implementados

- [x] Unity como engine (recomendado 6.1 LTS; usada versão 6.5, aceitável conforme edital)
- [x] Projeto estruturado em cenas diferentes (Menu e Navegador)
- [x] Navegação entre imagens via teclado (W/S, setas ↑/↓)
- [x] Navegação entre imagens via clique do mouse (botões Avançar/Voltar)
- [x] 10 imagens panorâmicas mapeadas e navegáveis (Av. Domingos Olímpio)
- [x] Build funcional para WebGL, testado em navegador real

**Bônus implementados:**
- [x] Feedback visual e sonoro nas ações (fade entre panoramas + som de transição)
- [x] Estilização da página/UI (identidade visual, tipografia e cores consistentes)

---

## Como rodar

1. Abrir a pasta do projeto no Unity Hub (versão 6.5 ou compatível)
2. Abrir a cena `Menu` e dar Play no Editor, **ou**
3. Acessar o build WebGL: [link do build/repositório aqui]
   - Importante: o build WebGL não roda com duplo clique no `index.html` (navegadores bloqueiam WebGL local por segurança). Servir com um servidor local (ex: Live Server do VS Code, ou `python -m http.server`).

**Controles:**
- `W` / `S` ou setas `↑` / `↓`: avançar / voltar entre panoramas
- Clique nos botões "Avançar" / "Voltar" na tela
- Tecla `Alt`: libera o cursor para clicar na UI (clicar fora da UI trava o cursor de volta ao modo de olhar ao redor)

---

## Arquitetura resumida

- **Cena Menu:** tela inicial, carrega a cena Navegador ao clicar em "Iniciar"
- **Cena Navegador:** contém toda a lógica de navegação
  - `NavigationManager.cs`: controla o índice do panorama atual, troca o `Skybox` da cena e dispara a transição (fade + som)
  - `CameraLook.cs`: controle de câmera em primeira pessoa (olhar ao redor) e gerenciamento do cursor
- **Sistema de navegação orientado a dados:** array `Material[] panoramas`, indexado por posição — adicionar uma imagem nova é adicionar um slot no array, sem alterar a lógica
- **Decisão de arquitetura central:** as imagens panorâmicas são aplicadas diretamente como `Skybox` da cena (shader `Skybox/Panoramic`), não em uma esfera 3D com mesh. O shader já foi feito para essa finalidade — isso eliminou a necessidade de qualquer geometria extra e simplificou o projeto.

---

## Diário de Bordo — Uso de Inteligência Artificial

> Nota de contexto: comecei este desafio com **zero experiência prévia em Unity e C#**. A ferramenta de IA foi usada como apoio para aprender a engine e debugar problemas em tempo real — não para gerar uma solução pronta sem entendimento.

### Ferramenta utilizada

**Claude (Anthropic)**, em conversa contínua ao longo de todo o desenvolvimento — desde os primeiros passos em Unity até os ajustes finais de UI e o bônus de transição.

### Como foi usado

O fluxo de trabalho seguia sempre o mesmo padrão: reproduzir o erro ou a dúvida → descrever o problema real (com print do Console/Editor quando havia erro) → entender a causa antes de aplicar qualquer correção → testar visualmente no Editor ou no navegador → só então considerar resolvido.

A IA foi usada para:
- Gerar os scripts C# principais (`CameraLook.cs`, `NavigationManager.cs`, `MenuManager.cs`)
- Diagnosticar erros de compilação e de build
- Explicar conceitos de Unity desconhecidos no início do desafio (Skybox, Anchor, Rect Transform, Input System)
- Sugerir soluções de UX (ex: o esquema da tecla Alt para liberar o cursor sem quebrar a navegação em primeira pessoa)

A IA **não** foi usada para:
- Decidir a localização real das imagens panorâmicas (escolha pessoal, por praticidade de deslocamento)
- Definir a composição artística do menu (paleta de cores, escolha da imagem de fundo)

### Prompts importantes (exemplos representativos)

- *"Como assim, tenho que conectar duas imagens?"* — dúvida sobre o conceito central do desafio, resolvida com uma analogia de livro-jogo ("escolha sua aventura")
- Envio direto de mensagens de erro do Console (ex: erros `CS0101`, `CS0111`, `CS0229`) pedindo a causa raiz antes da correção
- *"O que eu faço com esse botão?"* — dúvidas pontuais de UI que precisaram de explicação de conceito, não só de código

### Dificuldades reais enfrentadas

1. **Confusão entre arquivos `.cs`:** colei o código de um script dentro do arquivo de outro por engano, gerando 11 erros de compilação simultâneos. Aprendizado: cada script MonoBehaviour precisa estar em seu próprio arquivo, com o nome do arquivo igual ao nome da classe.
2. **Entendimento do conceito central do desafio:** não ficou claro de imediato que "navegar entre imagens" significava simular deslocamento real (como o Street View), e não apenas trocar imagens soltas.
3. **Configuração de Git com Unity:** avisos de LF/CRLF e arquivos de cache sendo indexados por engano por `.gitignore` mal posicionado.
4. **Erro de build WebGL não previsto:** compressão de arquivo (Gzip) quebrando o teste local com Live Server — resolvido isolando a causa por eliminação (`Compression Format = Disabled`).

### Como as respostas da IA foram validadas

Nenhuma sugestão de código ou configuração foi considerada "correta" apenas por parecer coerente — cada uma só foi aceita depois de testada visualmente no Unity Editor ou no navegador (build real). Quando um erro persistia, o processo era sempre voltar ao Console, capturar a mensagem exata, e reiniciar o ciclo de diagnóstico.

### Reflexão crítica

[Espaço para você escrever pessoalmente — sugestão de pontos a cobrir:]
- O que mudou na forma de usar a IA do início pro fim do desafio (ex: de "não entender o problema" para "debugar com vocabulário técnico próprio")
- Onde a IA acelerou o processo vs. onde exigiu mais cuidado/verificação da sua parte
- Algum momento em que uma sugestão da IA precisou ser ajustada ou rejeitada
- Como isso mudou (ou não) sua visão sobre usar IA em projetos futuros de jogos

---

## Estrutura de causa e efeito (decisões técnicas)

| Problema encontrado | Decisão tomada | Por quê |
|---|---|---|
| Shader Skybox/Panoramic não funcionava em uma esfera 3D | Abandonar a esfera, usar o Skybox da cena diretamente | O shader é feito para isso — arquitetura mais simples, sem geometria |
| Erro de Input System (projeto novo usa API nova por padrão) | Mudar Active Input Handling para "Both" | Evita reescrever com a API nova; aviso de depreciação não afeta o build |
| Cursor travado impedia clicar em botões de UI | Tecla Alt libera o cursor; clique fora da UI trava de volta | Resolve o conflito entre "olhar tipo FPS" e "clicar em botão" |
| Clique no botão "Avançar" não registrava | Checar `IsPointerOverGameObject()` antes de travar o cursor | Separa as duas intenções de clique |
| Botões flutuavam para posições erradas em telas diferentes | Trocar Anchor do centro para Anchor Presets nos cantos reais | Ancora no canto de verdade, não simula com posição a partir do centro |
| Build comprimido (Gzip) quebrava ao testar localmente | Compression Format = Disabled | Necessário só para o teste local; não afeta o requisito em si |

---

## Mapa de conexões das imagens

Local: Av. Domingos Olímpio, Fortaleza — trajeto reto, em direção ao centro (a partir do cruzamento com R. Jaime Benevides). Sequência linear simples: `panorama_01` é o início, cada número seguinte é "um passo à frente" do anterior, até `panorama_10`.

---

## Créditos e ferramentas de terceiros

- Imagens panorâmicas obtidas via [Street View Download 360](https://svd360.istreetview.com/)
- Efeito sonoro de transição: [preencher fonte/licença do arquivo `.wav` usado, ex: Freesound.org, CC0]
