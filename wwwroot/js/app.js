const API_URL = '/api/produtos';
let produtosCache = [];

document.addEventListener('DOMContentLoaded', () => {
    carregarProdutos();

    const form = document.getElementById('product-form');
    form.addEventListener('submit', salvarProduto);
});

async function carregarProdutos() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error('Erro ao buscar produtos.');

        produtosCache = await response.json();
        renderizarProdutos(produtosCache);
    } catch (error) {
        console.error(error);
        mostrarToast('Erro ao carregar lista de produtos.', 'error');
    }
}

function renderizarProdutos(produtos) {
    const tbody = document.getElementById('products-tbody');
    const emptyState = document.getElementById('empty-state');

    tbody.innerHTML = '';

    if (produtos.length === 0) {
        emptyState.style.display = 'block';
        return;
    }

    emptyState.style.display = 'none';

    produtos.forEach(p => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>#${p.id}</td>
            <td><strong>${escapeHtml(p.nome)}</strong></td>
            <td>${escapeHtml(p.categoria)}</td>
            <td>${escapeHtml(p.marca)}</td>
            <td>R$ ${Number(p.preco).toFixed(2).replace('.', ',')}</td>
            <td>${p.estoque} un.</td>
            <td>
                <div class="actions-cell">
                    <button class="btn-icon btn-edit" onclick="prepararEdicao(${p.id}, '${escapeQuote(p.nome)}', '${escapeQuote(p.categoria)}', '${escapeQuote(p.marca)}', ${p.preco}, ${p.estoque})" title="Editar">
                        <i class="fa-solid fa-pen-to-square"></i>
                    </button>
                    <button class="btn-icon btn-delete" onclick="deletarProduto(${p.id})" title="Excluir">
                        <i class="fa-solid fa-trash-can"></i>
                    </button>
                </div>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

async function salvarProduto(e) {
    e.preventDefault();

    const id = document.getElementById('produto-id').value;
    const nome = document.getElementById('nome').value.trim();
    const categoria = document.getElementById('categoria').value.trim();
    const marca = document.getElementById('marca').value.trim();
    const preco = parseFloat(document.getElementById('preco').value);
    const estoque = parseInt(document.getElementById('estoque').value);

    const payload = { nome, categoria, marca, preco, estoque };

    try {
        let response;
        if (id) {
            // Atualizar (PUT)
            response = await fetch(`${API_URL}/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
        } else {
            // Cadastrar (POST)
            response = await fetch(API_URL, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
        }

        if (!response.ok) {
            const errData = await response.json().catch(() => ({}));
            throw new Error(errData.mensagem || 'Erro ao salvar produto.');
        }

        mostrarToast(id ? 'Produto atualizado com sucesso!' : 'Produto cadastrado com sucesso!', 'success');
        cancelEdit();
        carregarProdutos();
    } catch (error) {
        console.error(error);
        mostrarToast(error.message, 'error');
    }
}

function prepararEdicao(id, nome, categoria, marca, preco, estoque) {
    document.getElementById('produto-id').value = id;
    document.getElementById('nome').value = nome;
    document.getElementById('categoria').value = categoria;
    document.getElementById('marca').value = marca;
    document.getElementById('preco').value = preco;
    document.getElementById('estoque').value = estoque;

    document.getElementById('form-title').innerHTML = '<i class="fa-solid fa-pen-to-square"></i> Editar Produto';
    document.getElementById('btn-save').innerHTML = '<i class="fa-solid fa-save"></i> Atualizar';
    document.getElementById('btn-cancel').style.display = 'inline-flex';

    window.scrollTo({ top: 0, behavior: 'smooth' });
}

function cancelEdit() {
    document.getElementById('product-form').reset();
    document.getElementById('produto-id').value = '';
    document.getElementById('form-title').innerHTML = '<i class="fa-solid fa-plus-circle"></i> Novo Produto';
    document.getElementById('btn-save').innerHTML = '<i class="fa-solid fa-save"></i> Salvar Produto';
    document.getElementById('btn-cancel').style.display = 'none';
}

async function deletarProduto(id) {
    if (!confirm('Tem certeza que deseja remover este produto?')) return;

    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Erro ao remover produto.');

        mostrarToast('Produto removido com sucesso!', 'success');
        carregarProdutos();
    } catch (error) {
        console.error(error);
        mostrarToast('Erro ao remover produto.', 'error');
    }
}

function filterProducts() {
    const query = document.getElementById('search-input').value.toLowerCase();
    const filtrados = produtosCache.filter(p =>
        p.nome.toLowerCase().includes(query) ||
        p.categoria.toLowerCase().includes(query) ||
        p.marca.toLowerCase().includes(query)
    );
    renderizarProdutos(filtrados);
}

function mostrarToast(mensagem, tipo = 'success') {
    const toast = document.getElementById('toast');
    toast.textContent = mensagem;
    toast.className = `toast show ${tipo}`;

    setTimeout(() => {
        toast.className = 'toast';
    }, 3500);
}

function escapeHtml(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

function escapeQuote(str) {
    return String(str).replace(/'/g, "\\'");
}
