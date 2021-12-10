let livros = [];
let autores = [];
let idSelecionado;
const ul = document.querySelector("ul");
const formLivro = document.getElementById("formLivro");
const formLogin = document.getElementById("formLogin");
const formUsuarios = document.getElementById("formUsuarios");
const buttonExcluir = document.getElementById("buttonExcluir");
const buttonCancelar = document.getElementById("buttonCancelar");
const buttonSalvar = document.getElementById("buttonSalvar");

async function init() {
  formLogin.addEventListener("submit", fazLogin);
  buttonCancelar.addEventListener("click", limpaSelecao);
  formLivro.addEventListener("submit", salvaLivro);
  buttonExcluir.addEventListener("click", deletarLivro);
  buttonCadastraAutor.addEventListener("click", cadastraAutor);
  buttonCadastraUsuario.addEventListener("click", cadastraUsuario);

  if (isLogado()) {
    mostraTelaLivros();
  } else {
    mostraTelaLogin();
  }
}
init();

function mostraTelaLogin() {
  document.getElementById("telaLivros").style.display = "none";
  document.getElementById("biblioteca").style.display = "none";
  document.getElementById("listaLivros").style.display = "none";
  document.getElementById("telaAdmin").style.display = "none";
  document.getElementById("telaLogin").style.display = "block";
  document.getElementById("errors").style.display = "none";
}

async function mostraTelaLivros() {
  document.getElementById("telaAdmin").style.display = "block";
  document.getElementById("biblioteca").style.display = "block";
  document.getElementById("telaLivros").style.display = "block";
  document.getElementById("listaLivros").style.display = "block";
  document.getElementById("telaLogin").style.display = "none";
  [livros, autores] = await Promise.all([listaLivros(), listaAutores()]);
  exibeOpcoesAutores();
  exibeLivros();
  limpaSelecao();
}

async function fazLogin(evt) {
  evt.preventDefault();
  try {
    await login(formLogin.email.value, formLogin.senha.value);
    mostraTelaLivros();
  } catch (erro) {
    mostraErro("Falha no login. Verifique e-mail e senha.");
  }
}

function selecionaItem(livro, li) {
  limpaSelecao();
  idSelecionado = livro.id;
  li.classList.add("selected");
  formLivro.descricao.value = livro.descricao;
  formLivro.isbn.value = livro.isbn;
  document.getElementById("isbn").setAttribute("disabled", "disabled");
  formLivro.autorId.value = livro.autor;
  document.getElementById("autorId").setAttribute("disabled", "disabled");
  formLivro.anoLancamento.valueAsNumber = livro.anoLancamento;
  buttonExcluir.style.display = "inline";
  buttonCancelar.style.display = "inline";
  buttonSalvar.textContent = "Atualizar";
}

function limpaSelecao() {
  limpaErros();
  idSelecionado = undefined;
  const li = ul.querySelector(".selected");
  if (li) {
    li.classList.remove("selected");
  }
  document.getElementById("isbn").removeAttribute("disabled");
  document.getElementById("autorId").removeAttribute("disabled");
  formLivro.descricao.value = "";
  formLivro.isbn.value = "";
  formLivro.autorId.value = "";
  formLivro.anoLancamento.value = "";
  buttonExcluir.style.display = "none";
  buttonCancelar.style.display = "none";
  buttonSalvar.textContent = "Cadastrar";
  formAutores.nomeAutor.value = "";
  formAutores.sobrenomeAutor.value = "";
}

async function salvaLivro(evt) {
  evt.preventDefault();
  const livroPayload = {
    id: parseInt(idSelecionado),
    descricao: formLivro.descricao.value,
    isbn: parseInt(formLivro.isbn.value),
    autorId: parseInt(formLivro.autorId.value),
    anoLancamento: parseInt(formLivro.anoLancamento.valueAsNumber),
  };
  if (idSelecionado) {
    await atualizaLivro(
      livroPayload.id,
      livroPayload.descricao,
      livroPayload.anoLancamento
    );
    livros = await listaLivros();
    exibeLivros();
  } else {
    if (
      !livroPayload.descricao ||
      !livroPayload.isbn ||
      !livroPayload.autorId ||
      !livroPayload.anoLancamento
    ) {
      mostraErro("Prencha todos os campos.");
    } else {
      try {
        await criaLivro(
          livroPayload.autorId,
          livroPayload.descricao,
          livroPayload.isbn,
          livroPayload.anoLancamento
        );
        livros = await listaLivros();
        exibeLivros();
        limpaSelecao();
      } catch (error) {
        mostraErro("Usuário sem autorização!");
      }
    }
  }
}

async function deletarLivro() {
  if (idSelecionado) {
    await excluiLivro(idSelecionado);
    livros = await listaLivros();
    limpaSelecao();
    exibeLivros();
  }
}

function exibeLivros() {
  ul.innerHTML = "";
  for (const livro of livros) {
    const li = document.createElement("li");
    
    const divAutor = document.createElement("div");
    divAutor.textContent = livro.nomeAutor;
    divAutor.className = "autorLivro";
    li.appendChild(divAutor);

    const divDescricao = document.createElement("div");
    divDescricao.textContent = livro.descricao;
    divDescricao.className = "descricaoLivro";
    li.appendChild(divDescricao);
    
    const divAno = document.createElement("div");
    divAno.textContent = livro.anoLancamento;
    divAno.className = "anoLivro";
    li.appendChild(divAno);
    ul.appendChild(li);
    if (idSelecionado === livro.id) {
      li.classList.add("selected");
    }
    li.addEventListener("click", () => selecionaItem(livro, li));
  }
}

function exibeOpcoesAutores() {
  formLivro.autorId.innerHTML = "";
  for (const autor of autores) {
    const option = document.createElement("option");
    option.textContent = autor.nome + " " + autor.sobrenome;
    option.value = autor.autorId;
    formLivro.autorId.appendChild(option);
  }
}

function mostraErro(message, error) {
  document.getElementById("errors").textContent = message;
  document.getElementById("errors").style.display = "block";
  if (error) {
    console.error(error);
  }
}

function limpaErros() {
  document.getElementById("errors").textContent = "";
  document.getElementById("errors").style.display = "none";
}

async function cadastraAutor(evt) {
  evt.preventDefault();
  const autorPayload = {
    nome: formAutores.nomeAutor.value,
    sobrenome: formAutores.sobrenomeAutor.value,
  };
  if (!autorPayload.nome || !autorPayload.sobrenome) {
    mostraErro("Prencha todos os campos.");
  } else {
    try {
      await criaAutor(autorPayload.nome, autorPayload.sobrenome);
      autores = await listaAutores();
      limpaSelecao();
      exibeOpcoesAutores();
    } catch (error) {
      mostraErro("Usuário sem autorização!");
    }
  }
}

async function cadastraUsuario(evt) {
  evt.preventDefault();
  const usuarioPayload = {
    roleId: parseInt(formUsuarios.role.value),
    nome: formUsuarios.nomeUsuario.value,
    email: formUsuarios.emailUsuario.value,
    senha: formUsuarios.senhaUsuario.value,
  };
  if (
    !usuarioPayload.roleId ||
    !usuarioPayload.nome ||
    !usuarioPayload.email ||
    !usuarioPayload.senha
  ) {
    mostraErro("Prencha todos os campos.");
  } else {
    try {
      await criaUsuario(
        usuarioPayload.roleId,
        usuarioPayload.nome,
        usuarioPayload.email,
        usuarioPayload.senha
      );
      limpaSelecao();
    } catch (error) {
      mostraErro("Usuário sem autorização!");
    }
  }
}
