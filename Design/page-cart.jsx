/* global React, Header, Footer, Icon, PRODUCTS, ProductCard */

function PageCart() {
  const [items, setItems] = React.useState([
    { id: 1, name: "Linen Throw Blanket", variant: "Natural · Throw", price: 128, qty: 1, img: PRODUCTS[0].img, stock: 14 },
    { id: 2, name: "Stoneware Mug, Set of 2", variant: "Bone white", price: 42, qty: 2, img: PRODUCTS[1].img, stock: 28 },
    { id: 3, name: "Brass Candlestick", variant: "Polished · Tall", price: 86, qty: 1, img: PRODUCTS[2].img, stock: 2 },
  ]);

  const update = (id, qty) => setItems(items.map(i => i.id === id ? { ...i, qty: Math.max(0, qty) } : i).filter(i => i.qty > 0));
  const subtotal = items.reduce((a, b) => a + b.price * b.qty, 0);
  const shipping = subtotal > 80 ? 0 : 8;

  return (
    <div className="eshop">
      <Header active="cart" cartCount={items.reduce((a,b)=>a+b.qty,0)} />

      <div className="container" style={{ padding: "40px 32px 0" }}>
        <h1 style={{ fontSize: 56 }}>Your cart</h1>
        <p style={{ color: "var(--muted)", marginTop: 8 }}>{items.length} items · review and check out</p>
      </div>

      <div className="container" style={{ padding: "32px", display: "grid", gridTemplateColumns: "1.6fr 1fr", gap: 56 }}>
        {/* Items list */}
        <div>
          <div style={{ display: "flex", flexDirection: "column", gap: 16 }}>
            {items.map(item => (
              <div key={item.id} className="surface" style={{ padding: 20, display: "grid", gridTemplateColumns: "120px 1fr auto", gap: 20, alignItems: "center" }}>
                <div style={{ aspectRatio: "1", borderRadius: 10, overflow: "hidden", background: "var(--surface-2)" }}>
                  <img src={item.img} style={{ width: "100%", height: "100%", objectFit: "cover" }} alt="" />
                </div>
                <div>
                  <div style={{ display: "flex", justifyContent: "space-between", gap: 12 }}>
                    <div>
                      <div style={{ fontFamily: "var(--font-display)", fontSize: 20 }}>{item.name}</div>
                      <div style={{ color: "var(--muted)", fontSize: 13, marginTop: 4 }}>{item.variant}</div>
                    </div>
                    <div className="price" style={{ fontSize: 18 }}>${(item.price * item.qty).toFixed(0)}</div>
                  </div>
                  <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginTop: 16 }}>
                    <div style={{ display: "flex", alignItems: "center", border: "1px solid var(--line-2)", borderRadius: 999, padding: 2 }}>
                      <button onClick={() => update(item.id, item.qty - 1)} style={{ width: 32, height: 32, borderRadius: 99, border: "none", background: "transparent", cursor: "pointer" }}><Icon.Minus /></button>
                      <span style={{ width: 28, textAlign: "center", fontWeight: 500, fontVariantNumeric: "tabular-nums" }}>{item.qty}</span>
                      <button onClick={() => update(item.id, item.qty + 1)} disabled={item.qty >= item.stock} style={{ width: 32, height: 32, borderRadius: 99, border: "none", background: "transparent", cursor: "pointer", opacity: item.qty >= item.stock ? 0.3 : 1 }}><Icon.Plus /></button>
                    </div>
                    <div style={{ display: "flex", alignItems: "center", gap: 16 }}>
                      {item.stock <= 3 && <span className="badge badge--warning">Only {item.stock} left</span>}
                      <button className="btn btn--link" style={{ fontSize: 13, color: "var(--muted)", borderBottomColor: "var(--muted)" }} onClick={() => update(item.id, 0)}>Remove</button>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>

          {/* Trust strip */}
          <div style={{ marginTop: 32, padding: "20px 24px", display: "flex", gap: 32, background: "var(--surface-2)", borderRadius: 14 }}>
            <TrustRow icon={<Icon.Truck />} text="Free shipping above $80" />
            <TrustRow icon={<Icon.Return />} text="30-day returns" />
            <TrustRow icon={<Icon.Shield />} text="Encrypted checkout" />
          </div>
        </div>

        {/* Summary */}
        <aside>
          <div style={{ position: "sticky", top: 96 }}>
            <div className="surface" style={{ padding: 28 }}>
              <h3 style={{ fontSize: 24, marginBottom: 20 }}>Summary</h3>
              <Line label="Subtotal" value={`$${subtotal.toFixed(2)}`} />
              <Line label="Shipping" value={shipping === 0 ? "Free" : `$${shipping.toFixed(2)}`} accent={shipping === 0} />
              <Line label="Estimated tax" value="$24.40" muted />
              <div style={{ height: 1, background: "var(--line)", margin: "16px 0" }} />
              <Line label="Total" value={`$${(subtotal + shipping + 24.40).toFixed(2)}`} large />

              <button className="btn btn--primary btn--lg btn--block" style={{ marginTop: 20 }}>
                Checkout securely <Icon.Arrow />
              </button>
              <div style={{ textAlign: "center", marginTop: 12, fontSize: 12, color: "var(--muted)" }}>
                Apple Pay · Google Pay · Visa · Mastercard
              </div>
            </div>

            <div style={{ marginTop: 20, padding: "20px 24px", background: "var(--accent-bg)", borderRadius: 14 }}>
              <div className="eyebrow" style={{ color: "var(--accent-ink)" }}>Promo code</div>
              <div style={{ display: "flex", gap: 8, marginTop: 10 }}>
                <input className="field" placeholder="Enter code" />
                <button className="btn btn--ghost btn--sm">Apply</button>
              </div>
            </div>
          </div>
        </aside>
      </div>

      {/* Cross-sell */}
      <section className="container" style={{ marginTop: 24, paddingBottom: 32 }}>
        <h2 style={{ fontSize: 28, marginBottom: 24 }}>Often bought with these</h2>
        <div style={{ display: "grid", gridTemplateColumns: "repeat(4, 1fr)", gap: 24 }}>
          {PRODUCTS.slice(3, 7).map(p => <ProductCard key={p.id} product={p} />)}
        </div>
      </section>

      <Footer />
    </div>
  );
}

function Line({ label, value, muted, accent, large }) {
  return (
    <div style={{ display: "flex", justifyContent: "space-between", padding: "8px 0", fontSize: large ? 17 : 14, color: muted ? "var(--muted)" : "var(--ink)", fontWeight: large ? 500 : 400 }}>
      <span>{label}</span>
      <span style={{ color: accent ? "var(--success)" : (large ? "var(--ink)" : undefined), fontFamily: large ? "var(--font-display)" : "var(--font-sans)", fontSize: large ? 24 : undefined }}>{value}</span>
    </div>
  );
}

function TrustRow({ icon, text }) {
  return (
    <div style={{ display: "flex", alignItems: "center", gap: 10, fontSize: 13, color: "var(--ink-2)" }}>
      <span style={{ color: "var(--accent)" }}>{icon}</span>{text}
    </div>
  );
}

window.PageCart = PageCart;
