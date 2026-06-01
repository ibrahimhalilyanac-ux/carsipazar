const { MongoClient } = require('mongodb');

async function main() {
    const url = 'mongodb://localhost:27017';
    const client = new MongoClient(url);

    try {
        await client.connect();
        console.log('Connected successfully to MongoDB');
        
        // List databases
        const adminDb = client.db().admin();
        const dbs = await adminDb.listDatabases();
        console.log('All Databases:', dbs.databases.map(d => d.name));

        for (let dbInfo of dbs.databases) {
            const dbName = dbInfo.name;
            if (dbName.toLowerCase().includes('shop') || dbName.toLowerCase().includes('catalog')) {
                const db = client.db(dbName);
                const cols = await db.listCollections().toArray();
                console.log(`Collections in DB [${dbName}]:`, cols.map(c => c.name));
                for (let col of cols) {
                    const count = await db.collection(col.name).countDocuments({});
                    console.log(`  Collection [${col.name}]: ${count} documents`);
                    if (count > 0) {
                        const docs = await db.collection(col.name).find({}).limit(2).toArray();
                        console.log(`    Docs:`, JSON.stringify(docs));
                    }
                }
            }
        }

    } catch (err) {
        console.error('Error:', err);
    } finally {
        await client.close();
    }
}

main();
