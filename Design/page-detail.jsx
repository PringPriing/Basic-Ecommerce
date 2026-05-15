/* global React, Header, Footer, ProductCard, PRODUCTS, Icon, Stars */

function PageProductDetail() {
  const [qty, setQty] = React.useState(1);
  const [imgIdx, setImgIdx] = React.useState(0);
  const product = PRODUCTS[0];
  const images = [
    "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=1000&auto=format&fit=crop&q=75",
    "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=1000&auto=format&fit=crop&q=75",
    "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=1000&auto=format&fit=crop&q=75",
    "https://images.unsplash.com/photo-1605883705077-8d3d3cebe78c?w=1000&auto=format&fit=crop&q=75",
  ];

  return (
    <div className="eshop">
      <Header active="shop" cartCount={2} />

      <div className="container" style={{ padding: "24px 32px 0" }}>
        <div style={{ fontSize: 12, color: "var(--muted)", display: "flex", gap: 8 }}>
          <a href="#" style={{ color: "var(--muted)", textDecoration: "none" }}>Home</a><span>/</span>
          <a href="#" style={{ color: "var(--muted)", textDecoration: "none" }}>Shop</a><span>/</span>
          <a href="#" style={{ color: "var(--muted)", textDecoration: "none" }}>Home textiles</a><span>/</span>
          <span style={{ color: "var(--ink)" }}>Linen Throw Blanket</span>
        </div>
      </div>

      <section className="container" style={{ padding: "32px", display: "grid", gridTemplateColumns: "80px 1fr 1fr", gap: 40 }}>
        {/* Thumbnail rail */}
        <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
          {images.map((img, i) => (
            <button key={i} onClick={() => setImgIdx(i)} style={{
              padding: 0, border: i === imgIdx ? "1.5px solid var(--ink)" : "1px solid var(--line)",
              borderRadius: 8, overflow: "hidden", cursor: "pointer", aspectRatio: "1", background: "var(--surface-2)"
            }}>
              <img src={img} style={{ width: "100%", height: "100%", objectFit: "cover", opacity: i === imgIdx ? 1 : 0.7 }} />
            </button>
          ))}
        </div>

        {/* Main image */}
        <div style={{ aspectRatio: "4 / 5", borderRadius: 14, overflow: "hidden", background: "var(--surface-2)", position: "relative" }}>
          <img src={images[imgIdx]} style={{ width: "100%", height: "100%", objectFit: "cover" }} alt="" />
          <span className="product-card__tag" style={{ top: 16, left: 16 }}>New arrival</span>
          <button className="eshop-header__icon-btn" style={{ position: "absolute", top: 12, right: 12, background: "var(--bg)" }}>
            <Icon.Heart />
          </button>
        </div>

        {/* Buy box */}
        <div style={{ padding: "8px 0 0" }}>
          <div className="eyebrow" style={{ marginBottom: 12 }}>Home textiles · Mado Linen Co.</div>
          <h1 style={{ fontSize: 44, marginBottom: 16 }}>Linen Throw Blanket</h1>
          <div style={{ display: "flex", alignItems: "center", gap: 12, marginBottom: 24 }}>
            <Stars value={5} count={84} />
            <span style={{ color: "var(--muted)", fontSize: 13 }}>· 92% would recommend</span>
          </div>

          <div style={{ display: "flex", alignItems: "baseline", gap: 12, marginBottom: 24 }}>
            <span className="price" style={{ fontSize: 36 }}>$128</span>
            <span style={{ color: "var(--muted)", textDecoration: "line-through", fontSize: 16 }}>$148</span>
            <span className="badge badge--accent">Save $20</span>
          </div>

          <p style={{ color: "var(--ink-2)", lineHeight: 1.7, marginBottom: 28 }}>
            Stonewashed 100% Belgian flax linen, woven in Vilnius and finished by hand. Soft from the first wash,
            it ages gracefully and gets better year after year. Generous throw size for a sofa, a guest room, or
            an afternoon nap.
          </p>

          {/* Color swatches */}
          <div style={{ marginBottom: 20 }}>
            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 10 }}>
              <span className="label" style={{ marginBottom: 0 }}>Color</span>
              <span style={{ fontSize: 13, color: "var(--muted)" }}>Natural</span>
            </div>
            <div style={{ display: "flex", gap: 10 }}>
              {[
                { name: "Natural", c: "#e8ddc8", active: true },
                { name: "Slate", c: "#6c7280" },
                { name: "Clay", c: "#b85c2e" },
                { name: "Olive", c: "#7a8260" },
              ].map(s => (
                <button key={s.name} title={s.name} style={{
                  width: 32, height: 32, borderRadius: 99,
                  background: s.c, border: s.active ? "2px solid var(--ink)" : "1px solid var(--line-2)",
                  outline: s.active ? "2px solid var(--bg)" : "none",
                  outlineOffset: -4, cursor: "pointer"
                }} />
              ))}
            </div>
          </div>

          {/* Size */}
          <div style={{ marginBottom: 28 }}>
            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 10 }}>
              <span className="label" style={{ marginBottom: 0 }}>Size</span>
              <a href="#" style={{ fontSize: 12, color: "var(--muted)" }}>Size guide ↗</a>
            </div>
            <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 8 }}>
              {[
                { l: "Throw", s: "50 × 60", active: true },
                { l: "Twin", s: "60 × 80" },
                { l: "Queen", s: "80 × 92", sold: true },
              ].map(o => (
                <button key={o.l} disabled={o.sold} style={{
                  padding: "12px 8px", textAlign: "left",
                  background: o.active ? "var(--ink)" : "var(--surface)",
                  color: o.active ? "var(--bg)" : (o.sold ? "var(--muted)" : "var(--ink)"),
                  border: o.active ? "1.5px solid var(--ink)" : "1px solid var(--line-2)",
                  borderRadius: 10, cursor: o.sold ? "not-allowed" : "pointer",
                  textDecoration: o.sold ? "line-through" : "none"
                }}>
                  <div style={{ fontWeight: 500, fontSize: 13 }}>{o.l}</div>
                  <div style={{ fontSize: 11, opacity: 0.7, marginTop: 2 }}>{o.s} in</div>
                </button>
              ))}
            </div>
          </div>

          {/* Qty + CTA */}
          <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
            <div style={{ display: "flex", alignItems: "center", border: "1px solid var(--line-2)", borderRadius: 999, padding: "4px" }}>
              <button onClick={()=>setQty(Math.max(1,qty-1))} style={{ width: 36, height: 36, borderRadius: 99, border: "none", background: "transparent", cursor: "pointer" }}><Icon.Minus /></button>
              <span style={{ width: 28, textAlign: "center", fontVariantNumeric: "tabular-nums", fontWeight: 500 }}>{qty}</span>
              <button onClick={()=>setQty(qty+1)} style={{ width: 36, height: 36, borderRadius: 99, border: "none", background: "transparent", cursor: "pointer" }}><Icon.Plus /></button>
            </div>
            <button className="btn btn--primary btn--lg" style={{ flex: 1 }}>Add to cart · $128</button>
          </div>

          <div style={{ display: "flex", gap: 8, marginBottom: 28 }}>
            <button className="btn btn--ghost btn--block">Add to wishlist</button>
            <button className="btn btn--ghost btn--block">Save for later</button>
          </div>

          {/* Stock */}
          <div style={{ display: "flex", alignItems: "center", gap: 12, padding: "14px 16px", background: "var(--success-bg)", borderRadius: 10, marginBottom: 24 }}>
            <span className="badge badge--success badge--dot">In stock</span>
            <span style={{ fontSize: 13, color: "var(--ink-2)" }}>14 units remaining · ships in 1–2 days</span>
          </div>

          {/* Specs */}
          <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 16, fontSize: 13, padding: "20px 0", borderTop: "1px solid var(--line)" }}>
            <Spec label="Material" value="100% Belgian flax linen" />
            <Spec label="Weight" value="240 gsm, medium" />
            <Spec label="Care" value="Machine wash cold" />
            <Spec label="Origin" value="Woven in Lithuania" />
          </div>
        </div>
      </section>

      {/* Related */}
      <section className="container" style={{ marginTop: 64, paddingBottom: 32 }}>
        <h2 style={{ fontSize: 32, marginBottom: 24 }}>You might also like</h2>
        <div style={{ display: "grid", gridTemplateColumns: "repeat(4, 1fr)", gap: 24 }}>
          {PRODUCTS.slice(1, 5).map(p => <ProductCard key={p.id} product={p} />)}
        </div>
      </section>

      <Footer />
    </div>
  );
}

function Spec({ label, value }) {
  return (
    <div>
      <div style={{ fontFamily: "var(--font-mono)", fontSize: 10, letterSpacing: "0.12em", textTransform: "uppercase", color: "var(--muted)", marginBottom: 4 }}>{label}</div>
      <div style={{ color: "var(--ink)" }}>{value}</div>
    </div>
  );
}

window.PageProductDetail = PageProductDetail;
