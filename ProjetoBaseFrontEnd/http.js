const baseUrl = "https://localhost:44394/api/v1";

function authHeaders() {
  return { Authorization: `Bearer ${sessionStorage.getItem("tokenAcesso")}` };
}

function isLogado() {
  return !!sessionStorage.getItem("tokenAcesso");
}

function login(email, senha) {
  return fetchJson(`${baseUrl}/Auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, senha }),
  }).then(
    (data) => {
      sessionStorage.setItem("tokenAcesso", data.tokenAcesso);
    },
    () => {
      sessionStorage.removeItem("tokenAcesso");
      throw new Error(
        "Falha no login. Verifique as credênciais e tente novamente."
      );
    }
  );
}

function fetchVoid(url, options) {
  return fetch(url, options).then((r) => {
    if (r.ok) {
      return;
    } else {
      throw new Error(r.statusText);
    }
  });
}

function fetchJson(url, options) {
  return fetch(url, options)
    .then((r) => {
      if (r.ok) {
        return r.json();
      } else {
        throw new Error(r.statusText);
      }
    })
    .then((json) => json.data);
}

function listaLivros() {
  return fetchJson(`${baseUrl}/Livros/listar`, { headers: authHeaders() });
}

function listaAutores() {
  return fetchJson(`${baseUrl}/Autores`, { headers: authHeaders() });
}

function criaAutor(nome, sobrenome) {
  return fetchVoid(`${baseUrl}/Autores`, {
    method: "POST",
    headers: { ...authHeaders(), "Content-Type": "application/json" },
    body: JSON.stringify({ nome, sobrenome }),
  });
}

function criaUsuario(roleId, nome, email, senha) {
  return fetchVoid(`${baseUrl}/Usuarios`, {
    method: "POST",
    headers: { ...authHeaders(), "Content-Type": "application/json" },
    body: JSON.stringify({ roleId, nome, email, senha }),
  });
}

function criaLivro(AutorId, Descricao, ISBN, AnoLancamento) {
  return fetchVoid(`${baseUrl}/Livros/cadastrar`, {
    method: "POST",
    headers: { ...authHeaders(), "Content-Type": "application/json" },
    body: JSON.stringify({
      AutorId: AutorId,
      Descricao: Descricao,
      ISBN: ISBN,
      AnoLancamento: AnoLancamento,
    }),
  });
}

function atualizaLivro(Id, Descricao, AnoLancamento) {
  return fetchVoid(`${baseUrl}/Livros/atualizar`, {
    method: "PUT",
    headers: { ...authHeaders(), "Content-Type": "application/json" },
    body: JSON.stringify({
      Id: Id,
      Descricao: Descricao,
      AnoLancamento: AnoLancamento,
    }),
  });
}

function excluiLivro(Id) {
  return fetchVoid(`${baseUrl}/Livros/deletar/${Id}`, {
    method: "DELETE",
    headers: { ...authHeaders(), "Content-Type": "application/json" },
  });
}
