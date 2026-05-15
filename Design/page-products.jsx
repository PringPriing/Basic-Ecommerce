/* global React, Header, Footer, ProductCard, PRODUCTS, Icon */

function PageProducts() {
  const [sort, setSort] = React.useState("featured");
  return (
    <div className="eshop">
      <Header active="shop" cartCount={2} />

      <div className="container" style={{ padding: "32px 32px 0" }}>
        <div style={{ fontSize: 12, color: "var(--muted)", display: "flex", gap: 8, marginBottom: 24 }}>
          <a href="#" style={{ color: "var(--muted)", textDecoration: "none" }}>Home</a>
          <span>/</span><span style={{ color: "var(--ink)" }}>All products</span>
        </div>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "end", marginBottom: 28 }}>
          <div>
            <h1 style={{ fontSize: 56 }}>All products</h1>
            <p style={{ color: "var(--muted)", marginTop: 8 }}>142 pieces, sorted by what's new this week.</p>
          </div>
        </div>

        {/* Search + sort row */}
        <div style={{ display: "flex", gap: 12, alignItems: "center", paddingBottom: 24, borderBottom: "1px solid var(--line)" }}>
          <div style={{ position: "relative", flex: 1, maxWidth: 420 }}>
            <span style={{ position: "absolute", left: 14, top: "50%", transform: "translateY(-50%)", color: "var(--muted)" }}><Icon.Search /></span>
            <input className="field" placeholder="Search by name, material, maker…" style={{ paddingLeft: 42 }} />
          </div>
          <div style={{ flex: 1 }} />
          <span style={{ fontSize: 13, color: "var(--muted)" }}>Sort</span>
          <select className="field" style={{ width: "auto", paddingRight: 32 }} value={sort} onChange={e=>setSort(e.target.value)}>
            <option>Featured</option>
            <option>Newest</option>
            <option>Price, low → high</option>
            <option>Price, high → low</option>
            <option>Best reviewed</option>
          </select>
        </div>
      </div>

      <div className="container" style={{ padding: "32px", display: "grid", gridTemplateColumns: "260px 1fr", gap: 48 }}>
        {/* Filters */}
        <aside>
          <FilterGroup title="Category">
            {["All", "Kitchen", "Home textiles", "Lighting", "Décor", "Bath", "Storage"].map((c, i) => (
              <FilterRow key={c} label={c} count={[142, 42, 28, 19, 36, 22, 15][i]} active={i === 0} />
            ))}
          </FilterGroup>
          <FilterGroup title="Price">
            <div style={{ display: "flex", gap: 8 }}>
              <input className="field" placeholder="Min" defaultValue="0" />
              <input className="field" placeholder="Max" defaultValue="500" />
            </div>
            <div style={{ marginTop: 16, height: 4, background: "var(--surface-2)", borderRadius: 99, position: "relative" }}>
              <div style={{ position: "absolute", left: "10%", right: "30%", top: 0, bottom: 0, background: "var(--ink)", borderRadius: 99 }} />
              <span style={{ position: "absolute", left: "calc(10% - 6px)", top: -4, width: 12, height: 12, background: "var(--bg)", border: "2px solid var(--ink)", borderRadius: 99 }} />
              <span style={{ position: "absolute", left: "calc(70% - 6px)", top: -4, width: 12, height: 12, background: "var(--bg)", border: "2px solid var(--ink)", borderRadius: 99 }} />
            </div>
          </FilterGroup>
          <FilterGroup title="Material">
            {["Linen", "Stoneware", "Brass", "Walnut", "Cotton", "Oak"].map(m => (
              <FilterRow key={m} label={m} checkbox />
            ))}
          </FilterGroup>
          <FilterGroup title="Rating">
            {[5,4,3].map(r => <FilterRow key={r} label={`${r} stars & up`} checkbox />)}
          </FilterGroup>
          <button className="btn btn--ghost btn--sm btn--block" style={{ marginTop: 16 }}>Clear all filters</button>
        </aside>

        {/* Grid */}
        <div>
          <div style={{ display: "flex", flexWrap: "wrap", gap: 8, marginBottom: 24 }}>
            <span className="badge">Linen <Icon.Close style={{ width: 12, height: 12, marginLeft: 4 }} /></span>
            <span className="badge">$0 – $200 <Icon.Close style={{ width: 12, height: 12, marginLeft: 4 }} /></span>
            <span style={{ color: "var(--muted)", fontSize: 13, marginLeft: 8 }}>Showing 9 of 28 results</span>
          </div>
          <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 28 }}>
            {PRODUCTS.map(p => <ProductCard key={p.id} product={p} tag={p.tag} />)}
          </div>
          <div style={{ marginTop: 56 }}>
            <div className="pagination">
              <button>‹</button>
              <button className="is-active">1</button>
              <button>2</button>
              <button>3</button>
              <button>…</button>
              <button>16</button>
              <button>›</button>
            </div>
          </div>
        </div>
      </div>

      <Footer />
    </div>
  );
}

function FilterGroup({ title, children }) {
  return (
    <div style={{ paddingBottom: 24, marginBottom: 24, borderBottom: "1px solid var(--line)" }}>
      <h4 style={{ fontFamily: "var(--font-mono)", fontSize: 11, letterSpacing: "0.12em", textTransform: "uppercase", color: "var(--ink)", margin: "0 0 14px", fontWeight: 500 }}>{title}</h4>
      <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>{children}</div>
    </div>
  );
}

function FilterRow({ label, count, active, checkbox }) {
  return (
    <label style={{ display: "flex", alignItems: "center", justifyContent: "space-between", gap: 8, fontSize: 14, color: active ? "var(--ink)" : "var(--ink-2)", fontWeight: active ? 500 : 400, cursor: "pointer" }}>
      <span style={{ display: "flex", alignItems: "center", gap: 10 }}>
        {checkbox && <span style={{ width: 14, height: 14, borderRadius: 3, border: "1.5px solid var(--line-2)", display: "inline-block" }} />}
        {label}
      </span>
      {count != null && <span style={{ fontSize: 12, color: "var(--muted)" }}>{count}</span>}
    </label>
  );
}

window.PageProducts = PageProducts;
